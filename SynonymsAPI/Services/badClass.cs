using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.StaticFiles;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Directory;
using Nop.Services.ExportImport.Help;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Shipping.Date;
using Nop.Services.Tax;
using Nop.Services.Vendors;
using OfficeOpenXml;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Nop.Services.Stores;
using Nop.Core.Infrastructure;
using System.Runtime.InteropServices;

namespace Nop.Services.ExportImport
{
    /// <summary>
    /// Import manager
    /// </summary>
    public partial class ImportManager : IImportManager
    {
        #region Constants

        //it's quite fast hash (to cheaply distinguish between objects)
        private const string IMAGE_HASH_ALGORITHM = "SHA1";

        private const string UPLOADS_TEMP_PATH = "~/App_Data/TempUploads";

        #endregion

        #region Fields

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IProductService _productService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IPictureService _pictureService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IStoreContext _storeContext;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IEncryptionService _encryptionService;
        private readonly IDataProvider _dataProvider;
        private readonly MediaSettings _mediaSettings;
        private readonly IVendorService _vendorService;
        private readonly IProductTemplateService _productTemplateService;
        private readonly IShippingService _shippingService;
        private readonly IDateRangeService _dateRangeService;
        private readonly ITaxCategoryService _taxCategoryService;
        private readonly IMeasureService _measureService;
        private readonly CatalogSettings _catalogSettings;
        private readonly IProductTagService _productTagService;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly VendorSettings _vendorSettings;
        private readonly ISpecificationAttributeService _specificationAttributeService;
        private readonly ILogger _logger;
        private readonly IStoreMappingService _storeMappingService;
        private readonly INopFileProvider _fileProvider;

        #endregion

        #region Ctor

        public ImportManager(IProductService productService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IPictureService pictureService,
            IUrlRecordService urlRecordService,
            IStoreContext storeContext,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IEncryptionService encryptionService,
            IDataProvider dataProvider,
            MediaSettings mediaSettings,
            IVendorService vendorService,
            IProductTemplateService productTemplateService,
            IShippingService shippingService,
            IDateRangeService dateRangeService,
            ITaxCategoryService taxCategoryService,
            IMeasureService measureService,
            IProductAttributeService productAttributeService,
            CatalogSettings catalogSettings,
            IProductTagService productTagService,
            IWorkContext workContext,
            ILocalizationService localizationService,
            ICustomerActivityService customerActivityService,
            VendorSettings vendorSettings,
            ISpecificationAttributeService specificationAttributeService,
            ILogger logger,
            IServiceScopeFactory serviceScopeFactory,
            IStoreMappingService storeMappingService,
            INopFileProvider fileProvider)
        {
            this._productService = productService;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._pictureService = pictureService;
            this._urlRecordService = urlRecordService;
            this._storeContext = storeContext;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._encryptionService = encryptionService;
            this._dataProvider = dataProvider;
            this._mediaSettings = mediaSettings;
            this._vendorService = vendorService;
            this._productTemplateService = productTemplateService;
            this._shippingService = shippingService;
            this._dateRangeService = dateRangeService;
            this._taxCategoryService = taxCategoryService;
            this._measureService = measureService;
            this._productAttributeService = productAttributeService;
            this._catalogSettings = catalogSettings;
            this._productTagService = productTagService;
            this._workContext = workContext;
            this._localizationService = localizationService;
            this._customerActivityService = customerActivityService;
            this._vendorSettings = vendorSettings;
            this._specificationAttributeService = specificationAttributeService;
            this._logger = logger;
            this._serviceScopeFactory = serviceScopeFactory;
            this._storeMappingService = storeMappingService;
            this._fileProvider = fileProvider;
        }

        #endregion

        #region Utilities

        private static ExportedAttributeType GetTypeOfExportedAttribute(ExcelWorksheet worksheet, PropertyManager<ExportProductAttribute> productAttributeManager, PropertyManager<ExportSpecificationAttribute> specificationAttributeManager, int iRow)
        {
            productAttributeManager.ReadFromXlsx(worksheet, iRow, ExportProductAttribute.ProducAttributeCellOffset);

            if (productAttributeManager.IsCaption)
            {
                return ExportedAttributeType.ProductAttribute;
            }

            specificationAttributeManager.ReadFromXlsx(worksheet, iRow, ExportProductAttribute.ProducAttributeCellOffset);

            if (specificationAttributeManager.IsCaption)
            {
                return ExportedAttributeType.SpecificationAttribute;
            }

            return ExportedAttributeType.NotSpecified;
        }

        private static void SetOutLineForSpecificationAttributeRow(object cellValue, ExcelWorksheet worksheet, int endRow)
        {
            var attributeType = (cellValue ?? string.Empty).ToString();

            if (attributeType.Equals("AttributeType", StringComparison.InvariantCultureIgnoreCase))
            {
                worksheet.Row(endRow).OutlineLevel = 1;
            }
            else
            {
                if (SpecificationAttributeType.Option.ToSelectList(useLocalization: false)
                    .Any(p => p.Text.Equals(attributeType, StringComparison.InvariantCultureIgnoreCase)))
                    worksheet.Row(endRow).OutlineLevel = 1;
            }
        }

        protected virtual int GetColumnIndex(string[] properties, string columnName)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            if (columnName == null)
                throw new ArgumentNullException(nameof(columnName));

            for (var i = 0; i < properties.Length; i++)
                if (properties[i].Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return i + 1; //excel indexes start from 1
            return 0;
        }

        protected virtual string ConvertColumnToString(object columnValue)
        {
            if (columnValue == null)
                return null;

            return Convert.ToString(columnValue);
        }

        protected virtual string GetMimeTypeFromFilePath(string filePath)
        {
            //TODO test ne implementation
            new FileExtensionContentTypeProvider().TryGetContentType(filePath, out string mimeType);
            //set to jpeg in case mime type cannot be found
            if (mimeType == null)
                mimeType = MimeTypes.ImageJpeg;
            return mimeType;
        }

        /// <summary>
        /// Creates or loads the image
        /// </summary>
        /// <param name="picturePath">The path to the image file</param>
        /// <param name="name">The name of the object</param>
        /// <param name="picId">Image identifier, may be null</param>
        /// <returns>The image or null if the image has not changed</returns>
        protected virtual Picture LoadPicture(string picturePath, string name, int? picId = null)
        {
            if (string.IsNullOrEmpty(picturePath) || !_fileProvider.FileExists(picturePath))
                return null;

            var mimeType = GetMimeTypeFromFilePath(picturePath);
            var newPictureBinary = _fileProvider.ReadAllBytes(picturePath);
            var pictureAlreadyExists = false;
            if (picId != null)
            {
                //compare with existing product pictures
                var existingPicture = _pictureService.GetPictureById(picId.Value);
                if (existingPicture != null)
                {
                    var existingBinary = _pictureService.LoadPictureBinary(existingPicture);
                    //picture binary after validation (like in database)
                    var validatedPictureBinary = _pictureService.ValidatePicture(newPictureBinary, mimeType);
                    if (existingBinary.SequenceEqual(validatedPictureBinary) ||
                        existingBinary.SequenceEqual(newPictureBinary))
                    {
                        pictureAlreadyExists = true;
                    }
                }
            }

            if (pictureAlreadyExists) return null;

            var newPicture = _pictureService.InsertPicture(newPictureBinary, mimeType, _pictureService.GetPictureSeName(name));
            return newPicture;
        }

        private void LogPictureInsertError(string picturePath, Exception ex)
        {
            var extension = _fileProvider.GetFileExtension(picturePath);
            var name = _fileProvider.GetFileNameWithoutExtension(picturePath);

            var point = string.IsNullOrEmpty(extension) ? string.Empty : ".";
            var fileName = _fileProvider.FileExists(picturePath) ? $"{name}{point}{extension}" : string.Empty;
            _logger.Error($"Insert picture failed (file name: {fileName})", ex);
        }

        protected virtual void ImportProductImagesUsingServices(IList<ProductPictureMetadata> productPictureMetadata)
        {
            foreach (var product in productPictureMetadata)
            {
                foreach (var picturePath in new[] { product.Picture1Path, product.Picture2Path, product.Picture3Path })
                {
                    if (string.IsNullOrEmpty(picturePath))
                        continue;

                    var mimeType = GetMimeTypeFromFilePath(picturePath);
                    var newPictureBinary = _fileProvider.ReadAllBytes(picturePath);
                    var pictureAlreadyExists = false;
                    if (!product.IsNew)
                    {
                        //compare with existing product pictures
                        var existingPictures = _pictureService.GetPicturesByProductId(product.ProductItem.Id);
                        foreach (var existingPicture in existingPictures)
                        {
                            var existingBinary = _pictureService.LoadPictureBinary(existingPicture);
                            //picture binary after validation (like in database)
                            var validatedPictureBinary = _pictureService.ValidatePicture(newPictureBinary, mimeType);
                            if (!existingBinary.SequenceEqual(validatedPictureBinary) &&
                                !existingBinary.SequenceEqual(newPictureBinary))
                                continue;
                            //the same picture content
                            pictureAlreadyExists = true;
                            break;
                        }
                    }

                    if (pictureAlreadyExists)
                        continue;

                    try
                    {
                        var newPicture = _pictureService.InsertPicture(newPictureBinary, mimeType, _pictureService.GetPictureSeName(product.ProductItem.Name));
                        product.ProductItem.ProductPictures.Add(new ProductPicture
                        {
                            //EF has some weird issue if we set "Picture = newPicture" instead of "PictureId = newPicture.Id"
                            //pictures are duplicated
                            //maybe because entity size is too large
                            PictureId = newPicture.Id,
                            DisplayOrder = 1,
                        });
                        _productService.UpdateProduct(product.ProductItem);
                    }
                    catch (Exception ex)
                    {
                        LogPictureInsertError(picturePath, ex);
                    }
                }
            }
        }

        protected virtual void ImportProductImagesUsingHash(IList<ProductPictureMetadata> productPictureMetadata, IList<Product> allProductsBySku)
        {
            //performance optimization, load all pictures hashes
            //it will only be used if the images are stored in the SQL Server database (not compact)
            var takeCount = _dataProvider.SupportedLengthOfBinaryHash() - 1;
            var productsImagesIds = _productService.GetProductsImagesIds(allProductsBySku.Select(p => p.Id).ToArray());
            var allPicturesHashes = _pictureService.GetPicturesHash(productsImagesIds.SelectMany(p => p.Value).ToArray());

            foreach (var product in productPictureMetadata)
            {
                foreach (var picturePath in new[] { product.Picture1Path, product.Picture2Path, product.Picture3Path })
                {
                    if (string.IsNullOrEmpty(picturePath))
                        continue;
                    try
                    {
                        var mimeType = GetMimeTypeFromFilePath(picturePath);
                        var newPictureBinary = _fileProvider.ReadAllBytes(picturePath);
                        var pictureAlreadyExists = false;
                        if (!product.IsNew)
                        {
                            var newImageHash = _encryptionService.CreateHash(newPictureBinary.Take(takeCount).ToArray(),
                                IMAGE_HASH_ALGORITHM);
                            var newValidatedImageHash = _encryptionService.CreateHash(_pictureService.ValidatePicture(newPictureBinary, mimeType)
                                .Take(takeCount)
                                .ToArray(), IMAGE_HASH_ALGORITHM);

                            var imagesIds = productsImagesIds.ContainsKey(product.ProductItem.Id)
                                ? productsImagesIds[product.ProductItem.Id]
                                : new int[0];

                            pictureAlreadyExists = allPicturesHashes.Where(p => imagesIds.Contains(p.Key))
                                .Select(p => p.Value).Any(p => p == newImageHash || p == newValidatedImageHash);
                        }

                        if (pictureAlreadyExists)
                            continue;

                        var newPicture = _pictureService.InsertPicture(newPictureBinary, mimeType, _pictureService.GetPictureSeName(product.ProductItem.Name));
                        product.ProductItem.ProductPictures.Add(new ProductPicture
                        {
                            //EF has some weird issue if we set "Picture = newPicture" instead of "PictureId = newPicture.Id"
                            //pictures are duplicated
                            //maybe because entity size is too large
                            PictureId = newPicture.Id,
                            DisplayOrder = 1,
                        });
                        _productService.UpdateProduct(product.ProductItem);
                    }
                    catch (Exception ex)
                    {
                        LogPictureInsertError(picturePath, ex);
                    }
                }
            }
        }

        protected virtual string UpdateCategoryByXlsx(Category category, PropertyManager<Category> manager, Dictionary<string, Category> allCategories, bool isNew, out bool isParentCategoryExists)
        {
            var seName = string.Empty;
            isParentCategoryExists = true;
            var isParentCategorySet = false;

            foreach (var property in manager.GetProperties)
            {
                switch (property.PropertyName)
                {
                    case "Name":
                        category.Name = property.StringValue.Split(new[] { ">>" }, StringSplitOptions.RemoveEmptyEntries).Last().Trim();
                        break;
                    case "Description":
                        category.Description = property.StringValue;
                        break;
                    case "CategoryTemplateId":
                        category.CategoryTemplateId = property.IntValue;
                        break;
                    case "MetaKeywords":
                        category.MetaKeywords = property.StringValue;
                        break;
                    case "MetaDescription":
                        category.MetaDescription = property.StringValue;
                        break;
                    case "MetaTitle":
                        category.MetaTitle = property.StringValue;
                        break;
                    case "ParentCategoryId":
                        if (!isParentCategorySet)
                        {
                            var parentCategory = allCategories.Values.FirstOrDefault(c => c.Id == property.IntValue);
                            isParentCategorySet = parentCategory != null;

                            isParentCategoryExists = isParentCategorySet || property.IntValue == 0;

                            category.ParentCategoryId = parentCategory?.Id ?? property.IntValue;
                        }
                        break;
                    case "ParentCategoryName":
                        if (_catalogSettings.ExportImportCategoriesUsingCategoryName && !isParentCategorySet)
                        {
                            var categoryName = manager.GetProperty("ParentCategoryName").StringValue;
                            if (!string.IsNullOrEmpty(categoryName))
                            {
                                var parentCategory = allCategories.ContainsKey(categoryName)
                                    //try find category by full name with all parent category names
                                    ? allCategories[categoryName]
                                    //try find category by name
                                    : allCategories.Values.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.InvariantCulture));

                                if (parentCategory != null)
                                {
                                    category.ParentCategoryId = parentCategory.Id;
                                    isParentCategorySet = true;
                                }
                                else
                                {
                                    isParentCategoryExists = false;
                                }
                            }
                        }
                        break;
                    case "Picture":
                        var picture = LoadPicture(manager.GetProperty("Picture").StringValue, category.Name, isNew ? null : (int?)category.PictureId);
                        if (picture != null)
                            category.PictureId = picture.Id;
                        break;
                    case "PageSize":
                        category.PageSize = property.IntValue;
                        break;
                    case "AllowCustomersToSelectPageSize":
                        category.AllowCustomersToSelectPageSize = property.BooleanValue;
                        break;
                    case "PageSizeOptions":
                        category.PageSizeOptions = property.StringValue;
                        break;
                    case "PriceRanges":
                        category.PriceRanges = property.StringValue;
                        break;
                    case "ShowOnHomePage":
                        category.ShowOnHomePage = property.BooleanValue;
                        break;
                    case "IncludeInTopMenu":
                        category.IncludeInTopMenu = property.BooleanValue;
                        break;
                    case "Published":
                        category.Published = property.BooleanValue;
                        break;
                    case "DisplayOrder":
                        category.DisplayOrder = property.IntValue;
                        break;
                    case "SeName":
                        seName = property.StringValue;
                        break;
                }
            }

            category.UpdatedOnUtc = DateTime.UtcNow;
            return seName;
        }

        protected virtual Category GetCategoryFromXlsx(PropertyManager<Category> manager, ExcelWorksheet worksheet, int iRow, Dictionary<string, Category> allCategories, out bool isNew, out string curentCategoryBreadCrumb)
        {
            manager.ReadFromXlsx(worksheet, iRow);

            //try get category from database by ID
            var category = allCategories.Values.FirstOrDefault(c => c.Id == manager.GetProperty("Id")?.IntValue);

            if (_catalogSettings.ExportImportCategoriesUsingCategoryName && category == null)
            {
                var categoryName = manager.GetProperty("Name").StringValue;
                if (!string.IsNullOrEmpty(categoryName))
                {
                    category = allCategories.ContainsKey(categoryName)
                        //try find category by full name with all parent category names
                        ? allCategories[categoryName]
                        //try find category by name
                        : allCategories.Values.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.InvariantCulture));
                }
            }

            isNew = category == null;

            category = category ?? new Category();

            curentCategoryBreadCrumb = string.Empty;

            if (isNew)
            {
                category.CreatedOnUtc = DateTime.UtcNow;
                //default values
                category.PageSize = _catalogSettings.DefaultCategoryPageSize;
                category.PageSizeOptions = _catalogSettings.DefaultCategoryPageSizeOptions;
                category.Published = true;
                category.IncludeInTopMenu = true;
                category.AllowCustomersToSelectPageSize = true;
            }
            else
            {
                curentCategoryBreadCrumb = category.GetFormattedBreadCrumb(_categoryService);
            }

            return category;
        }

        protected virtual void SaveCategory(bool isNew, Category category, Dictionary<string, Category> allCategories, string curentCategoryBreadCrumb, bool setSeName, string seName)
        {
            if (isNew)
                _categoryService.InsertCategory(category);
            else
                _categoryService.UpdateCategory(category);

            var categoryBreadCrumb = category.GetFormattedBreadCrumb(_categoryService);
            if (!allCategories.ContainsKey(categoryBreadCrumb))
                allCategories.Add(categoryBreadCrumb, category);
            if (!string.IsNullOrEmpty(curentCategoryBreadCrumb) && allCategories.ContainsKey(curentCategoryBreadCrumb) &&
                categoryBreadCrumb != curentCategoryBreadCrumb)
                allCategories.Remove(curentCategoryBreadCrumb);

            //search engine name
            if (setSeName)
                _urlRecordService.SaveSlug(category, category.ValidateSeName(seName, category.Name, true), 0);
        }

        protected virtual void SetOutLineForProductAttributeRow(object cellValue, ExcelWorksheet worksheet, int endRow)
        {
            try
            {
                var aid = Convert.ToInt32(cellValue ?? -1);

                var productAttribute = _productAttributeService.GetProductAttributeById(aid);

                if (productAttribute != null)
                    worksheet.Row(endRow).OutlineLevel = 1;
            }
            catch (FormatException)
            {
                if ((cellValue ?? string.Empty).ToString() == "AttributeId")
                    worksheet.Row(endRow).OutlineLevel = 1;
            }
        }

        protected virtual void ImportProductAttribute(PropertyManager<ExportProductAttribute> productAttributeManager, Product lastLoadedProduct)
        {
            if (!_catalogSettings.ExportImportProductAttributes || lastLoadedProduct == null || productAttributeManager.IsCaption)
                return;

            var productAttributeId = productAttributeManager.GetProperty("AttributeId").IntValue;
            var attributeControlTypeId = productAttributeManager.GetProperty("AttributeControlType").IntValue;

            var productAttributeValueId = productAttributeManager.GetProperty("ProductAttributeValueId").IntValue;
            var associatedProductId = productAttributeManager.GetProperty("AssociatedProductId").IntValue;
            var valueName = productAttributeManager.GetProperty("ValueName").StringValue;
            var attributeValueTypeId = productAttributeManager.GetProperty("AttributeValueType").IntValue;
            var colorSquaresRgb = productAttributeManager.GetProperty("ColorSquaresRgb").StringValue;
            var imageSquaresPictureId = productAttributeManager.GetProperty("ImageSquaresPictureId").IntValue;
            var priceAdjustment = productAttributeManager.GetProperty("PriceAdjustment").DecimalValue;
            var priceAdjustmentUsePercentage = productAttributeManager.GetProperty("PriceAdjustmentUsePercentage").BooleanValue;
            var weightAdjustment = productAttributeManager.GetProperty("WeightAdjustment").DecimalValue;
            var cost = productAttributeManager.GetProperty("Cost").DecimalValue;
            var customerEntersQty = productAttributeManager.GetProperty("CustomerEntersQty").BooleanValue;
            var quantity = productAttributeManager.GetProperty("Quantity").IntValue;
            var isPreSelected = productAttributeManager.GetProperty("IsPreSelected").BooleanValue;
            var displayOrder = productAttributeManager.GetProperty("DisplayOrder").IntValue;
            var pictureId = productAttributeManager.GetProperty("PictureId").IntValue;
            var textPrompt = productAttributeManager.GetProperty("AttributeTextPrompt").StringValue;
            var isRequired = productAttributeManager.GetProperty("AttributeIsRequired").BooleanValue;
            var attributeDisplayOrder = productAttributeManager.GetProperty("AttributeDisplayOrder").IntValue;

            var productAttributeMapping =
                lastLoadedProduct.ProductAttributeMappings.FirstOrDefault(
                    pam => pam.ProductAttributeId == productAttributeId);

            if (productAttributeMapping == null)
            {
                //insert mapping
                productAttributeMapping = new ProductAttributeMapping
                {
                    ProductId = lastLoadedProduct.Id,
                    ProductAttributeId = productAttributeId,
                    TextPrompt = textPrompt,
                    IsRequired = isRequired,
                    AttributeControlTypeId = attributeControlTypeId,
                    DisplayOrder = attributeDisplayOrder
                };
                _productAttributeService.InsertProductAttributeMapping(productAttributeMapping);
            }
            else
            {
                productAttributeMapping.AttributeControlTypeId = attributeControlTypeId;
                productAttributeMapping.TextPrompt = textPrompt;
                productAttributeMapping.IsRequired = isRequired;
                productAttributeMapping.DisplayOrder = attributeDisplayOrder;
                _productAttributeService.UpdateProductAttributeMapping(productAttributeMapping);
            }

            var pav = _productAttributeService.GetProductAttributeValueById(productAttributeValueId);

            var attributeControlType = (AttributeControlType)attributeControlTypeId;

            if (pav == null)
            {
                switch (attributeControlType)
                {
                    case AttributeControlType.Datepicker:
                    case AttributeControlType.FileUpload:
                    case AttributeControlType.MultilineTextbox:
                    case AttributeControlType.TextBox:
                        return;
                }

                pav = new ProductAttributeValue
                {
                    ProductAttributeMappingId = productAttributeMapping.Id,
                    AttributeValueType = (AttributeValueType)attributeValueTypeId,
                    AssociatedProductId = associatedProductId,
                    Name = valueName,
                    PriceAdjustment = priceAdjustment,
                    PriceAdjustmentUsePercentage = priceAdjustmentUsePercentage,
                    WeightAdjustment = weightAdjustment,
                    Cost = cost,
                    IsPreSelected = isPreSelected,
                    DisplayOrder = displayOrder,
                    ColorSquaresRgb = colorSquaresRgb,
                    ImageSquaresPictureId = imageSquaresPictureId,
                    CustomerEntersQty = customerEntersQty,
                    Quantity = quantity,
                    PictureId = pictureId
                };

                _productAttributeService.InsertProductAttributeValue(pav);
            }
            else
            {
                pav.AttributeValueTypeId = attributeValueTypeId;
                pav.AssociatedProductId = associatedProductId;
                pav.Name = valueName;
                pav.ColorSquaresRgb = colorSquaresRgb;
                pav.ImageSquaresPictureId = imageSquaresPictureId;
                pav.PriceAdjustment = priceAdjustment;
                pav.PriceAdjustmentUsePercentage = priceAdjustmentUsePercentage;
                pav.WeightAdjustment = weightAdjustment;
                pav.Cost = cost;
                pav.CustomerEntersQty = customerEntersQty;
                pav.Quantity = quantity;
                pav.IsPreSelected = isPreSelected;
                pav.DisplayOrder = displayOrder;
                pav.PictureId = pictureId;

                _productAttributeService.UpdateProductAttributeValue(pav);
            }
        }

        #endregion
    }
}