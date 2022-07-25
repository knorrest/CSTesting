import React from "react";
import "./form.css";

function TextField(props) {
  const { id, label, className, value, placeholder, disabled } = props;

  const handleChange = (e) => {
    props.onChange(e);
  };

  return (
    <div className={className}>
      <label disabled={disabled}>{label}</label>
      <input
        id={id}
        type="text"
        placeholder={placeholder}
        value={value != null ? value : ""}
        onChange={handleChange}
        className="input"
      />
    </div>
  );
}
export default TextField;
