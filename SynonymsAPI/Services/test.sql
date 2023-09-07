-- name: delete-project-by-id!
-- Deletes the project as identified by the id
delete from projects where id = ?;

-- name: delete-project-analysis-configuration-by-id!
delete from project_analysis_configurations where id = ?;

--name: delete-complexity-trend-config-by-id!
delete from project_complexity_trends_configuration where id = ?;

-- name: delete-rising-hotspot-config-by-id!
delete from project_rising_hotspots_configuration where id = ?;

-- name: delete-analysis-status-by-project-id!
delete from analysis_status where project_id = ?;

-- name: delete-project-system-warnings-by-project-id!
delete from project_system_warnings  where project_id = ?;

-- name: delete-project-repository-paths-by-project-id!
delete from project_repository_paths
  where project_id = ? and path not IN (:exclude_paths) ;

-- name: delete-architectural-transforms-by-project-id!
-- Deletes the transformation as identified by the id of its project.
delete from project_architectural_transformations where project_id = ?;

-- name: delete-all-project-repository-paths!
delete from project_repository_paths;

-- name: delete-all-project-exclude-commits!
delete from project_exclude_commits;

-- name: delete-all-projects!
-- Deletes all existing projects.
delete from projects;

-- name: delete-all-complexity-trend-configurations!
-- Deletes all trend configurations.
delete from project_complexity_trends_configuration;

--name: delete-all-rising-hotspot-configurations!
-- Deletes all configuration values for rising spots.
delete from project_rising_hotspots_configuration;

--name: delete-code-comments-biomarker-config-by-project-id!
delete from project_code_comment_biomarkers where project_id = ?;

--name: select-code-comments-by-project-id
select code_comment from project_code_comment_biomarkers where project_id = ?;

-- name: insert-code-comments-by-project-id!
insert into project_code_comment_biomarkers (project_id, code_comment) values(:project_id, :code_comment);

-- name: select-repository-paths-by-project-id
-- Retrieves the paths to all repositories that belong to one project.
select * from project_repository_paths where project_id = ?;

-- name: select-repository-paths
select * from project_repository_paths;

-- name: project-by-name
-- The project as identified by its name.
select p.id, p.name, p.path, p.description, p.project_owner, p.fixed_repo_roots,
       p.code_comment_enabled, p.created_at,
       p.fixed_results_root, p.analysisdest, p.ticket_id_uri_template, c.project_start_date, c.firstanalysisdate, c.analysisplan, s.analysistype,
       c.minrevs, c.maxchangesetsize, c.rollingaveragedays, c.update_repositories, c.fail_on_update_repositories,
       c.exclusionfilter, c.excludecontent, c.whitelistcontent, c.gitremotelocalpath,
       c.delta_recommend_review_level,
       c.delta_always_comment,
       c.delta_ignore_other_branches,
       c.delta_coupling_threshold,
       c.delta_branch_name_exclusion_pattern,
       c.xrayallrevisions,
       c.xray_min_coupling_degree,
       c.x_ray_separate_overloads,
       c.use_parallel_code_health_strategy,
       c.scan_all_code_in_project,
       c.code_health_alert_level,
       rising.spottrendspan,
       s.status, c.selectdistinctcommits, c.ticketidpattern, c.temporal_coupling_strategy,
       c.minexdevcontribtime, c.exdevcontribenabled,
       c.systemleveltrend,
       c.parallelgitmining,
       c.pair_programming_pattern,
       c.pair_programming_author_message_pattern,
       c.combine_authors_field_and_commit_msg_to_deduce_pairs,
       c.pm_data_remove_outliers,
       c.delivery_performance_strategy,
       c.delivery_performance_release_tag,
       c.temporal_coupling_across_minrevs, c.temporal_coupling_archlevel_across_minrevs,
       c.temporal_coupling_across_maxchangesetsize,
       c.modus_commit_message_pattern,
       c.complete_history_for_social,
       c.auto_detect_text_files, c.lookup_initial_file_size,
       c.hotspots_sliding_window_time,
       c.pm_data_sliding_window_time,
       c.team_analysis_sliding_window_time,
       c.team_analysis_start_date,
       c.age_is_last_commit,
       c.branch_delivery_risk_commit_limit,
       c.branch_name_exclusion_pattern,
       c.branches_analysis_lookback_weeks,
       c.calculate_branch_statistics,
       c.git_rename_limit,
       c.hotspots_defect_mining_pattern,
       c.code_ownership_source_file_names,
       c.disable_code_health_custom_rules,
       c.global_code_health_custom_rules,
       complexity.complexitytrendwarninglookbackinmonths,
       p.do_housekeeping, p.max_historic_age, p.max_history_depth
       from projects as p
       join project_analysis_configurations as c
            on p.config = c.id
       join analysis_status as s
            on p.id = s.project_id
       join project_complexity_trends_configuration as complexity
            on p.complexitytrend = complexity.id
       join project_rising_hotspots_configuration as rising
            on p.risingspots = rising.id
        where name = ?;

-- name: project-by-id
-- The project as identified by its primary key.
select p.id, p.name, p.path, p.description, p.project_owner,
       p.code_comment_enabled, p.created_at,
       p.fixed_repo_roots, p.fixed_results_root, p.analysisdest, p.ticket_id_uri_template, c.project_start_date, c.firstanalysisdate, c.analysisplan, s.analysistype,
       c.minrevs, c.maxchangesetsize, c.rollingaveragedays, c.update_repositories, c.fail_on_update_repositories,
       c.exclusionfilter, c.excludecontent, c.whitelistcontent, c.gitremotelocalpath,
       c.delta_recommend_review_level,
       c.delta_always_comment,
       c.delta_ignore_other_branches,
       c.delta_coupling_threshold,
       c.delta_branch_name_exclusion_pattern,
       c.xrayallrevisions,
       c.xray_min_coupling_degree,
       c.x_ray_separate_overloads,
       c.use_parallel_code_health_strategy,
       c.scan_all_code_in_project,
       c.code_health_alert_level,
       rising.spottrendspan,
       s.status,
       c.systemleveltrend,
       c.parallelgitmining,
       c.pair_programming_pattern,
       c.pair_programming_author_message_pattern,
       c.combine_authors_field_and_commit_msg_to_deduce_pairs,
       c.pm_data_remove_outliers,
       c.delivery_performance_strategy,
       c.delivery_performance_release_tag,
       p.config, p.complexitytrend, p.risingspots,
       c.selectdistinctcommits, c.ticketidpattern,
       c.minexdevcontribtime, c.exdevcontribenabled, c.temporal_coupling_strategy,
       c.temporal_coupling_across_minrevs, c.temporal_coupling_archlevel_across_minrevs,
       c.temporal_coupling_across_maxchangesetsize,
       c.modus_commit_message_pattern,
       c.complete_history_for_social,
       c.auto_detect_text_files, c.lookup_initial_file_size,
       c.hotspots_sliding_window_time,
       c.pm_data_sliding_window_time,
       c.team_analysis_sliding_window_time,
       c.team_analysis_start_date,
       c.age_is_last_commit,
       c.branch_delivery_risk_commit_limit,
       c.branch_name_exclusion_pattern,
       c.branches_analysis_lookback_weeks,
       c.calculate_branch_statistics,
       c.git_rename_limit,
       c.hotspots_defect_mining_pattern,
       c.code_ownership_source_file_names,
       c.disable_code_health_custom_rules,
       c.global_code_health_custom_rules,
       complexity.complexitytrendwarninglookbackinmonths,
       p.do_housekeeping, p.max_historic_age, p.max_history_depth,
       pg.id group_id,
       pg.name group_name
       from projects as p
       join project_analysis_configurations as c
            on p.config = c.id
       join analysis_status as s
            on p.id = s.project_id
       join project_complexity_trends_configuration as complexity
            on p.complexitytrend = complexity.id
       join project_rising_hotspots_configuration as rising
            on p.risingspots = rising.id
       left join project_groups_assignments as pga
            on p.id = pga.project_id
       left join project_groups as pg
            on pga.group_id = pg.id
        where p.id = ?;

-- name: get-project-list
-- Returns a lightweight list of all projects sorted by name
select p.id, p.name, p.description, s.status, s.last_status_change, s.analysis_path, pg.name group_name, pg.id group_id
       from projects as p
       join analysis_status as s
            on p.id = s.project_id
       left join project_groups_assignments as pga
            on p.id = pga.project_id
       left join project_groups as pg
            on pga.group_id = pg.id;

-- name: get-all-projects
-- Returns all projects sorted on their name.
select p.id, p.name, p.path, p.description, p.project_owner,
       p.code_comment_enabled, p.created_at,
       p.fixed_repo_roots, p.fixed_results_root, p.analysisdest, p.ticket_id_uri_template, c.project_start_date, c.firstanalysisdate, c.analysisplan, s.analysistype,
       c.minrevs, c.maxchangesetsize, c.rollingaveragedays, c.update_repositories, c.fail_on_update_repositories,
       c.exclusionfilter, c.excludecontent, c.whitelistcontent, c.gitremotelocalpath,
       c.delta_recommend_review_level,
       c.delta_always_comment,
       c.delta_ignore_other_branches,
       c.delta_coupling_threshold,
       c.delta_branch_name_exclusion_pattern,
       c.xrayallrevisions,
       c.xray_min_coupling_degree,
       c.x_ray_separate_overloads,
       c.use_parallel_code_health_strategy,
       c.scan_all_code_in_project,
       c.code_health_alert_level,
       rising.spottrendspan,
       s.status,
       c.selectdistinctcommits, c.ticketidpattern,
       c.minexdevcontribtime, c.exdevcontribenabled, c.temporal_coupling_strategy,
       c.temporal_coupling_across_minrevs, c.temporal_coupling_archlevel_across_minrevs,
       c.temporal_coupling_across_maxchangesetsize,
       c.systemleveltrend,
       c.parallelgitmining,
       c.pair_programming_pattern,
       c.pair_programming_author_message_pattern,
       c.combine_authors_field_and_commit_msg_to_deduce_pairs,
       c.pm_data_remove_outliers,
       c.delivery_performance_strategy,
       c.delivery_performance_release_tag,
       c.modus_commit_message_pattern,
       c.complete_history_for_social,
       c.auto_detect_text_files, c.lookup_initial_file_size,
       c.hotspots_sliding_window_time,
       c.pm_data_sliding_window_time,
       c.team_analysis_sliding_window_time,
       c.team_analysis_start_date,
       c.age_is_last_commit,
       c.branch_delivery_risk_commit_limit,
       c.branch_name_exclusion_pattern,
       c.branches_analysis_lookback_weeks,
       c.calculate_branch_statistics,
       c.git_rename_limit,
       c.hotspots_defect_mining_pattern,
       c.code_ownership_source_file_names,
       c.disable_code_health_custom_rules,
       c.global_code_health_custom_rules,
       complexity.complexitytrendwarninglookbackinmonths,
       p.do_housekeeping, p.max_historic_age, p.max_history_depth,
       pg.name group_name,
       pg.id group_id
       from projects as p
       join project_analysis_configurations as c
            on p.config = c.id
       join analysis_status as s
            on p.id = s.project_id
       join project_complexity_trends_configuration as complexity
            on p.complexitytrend = complexity.id
       join project_rising_hotspots_configuration as rising
            on p.risingspots = rising.id
       left join project_groups_assignments as pga
            on p.id = pga.project_id
       left join project_groups as pg
            on pga.group_id = pg.id
       order by p.name asc;


-- name: get-projects-by-ids
-- Returns all projects with the given ids, sorted on their name.
select p.id, p.name, p.path, p.description, p.project_owner,
       p.code_comment_enabled, p.created_at,
       p.fixed_repo_roots, p.fixed_results_root, p.analysisdest, p.ticket_id_uri_template, c.project_start_date, c.firstanalysisdate, c.analysisplan, s.analysistype,
       c.minrevs, c.maxchangesetsize, c.rollingaveragedays, c.update_repositories, c.fail_on_update_repositories,
       c.exclusionfilter, c.excludecontent, c.whitelistcontent, c.gitremotelocalpath,
       c.delta_recommend_review_level,
       c.delta_always_comment,
       c.delta_ignore_other_branches,
       c.delta_coupling_threshold,
       c.delta_branch_name_exclusion_pattern,
       c.xrayallrevisions,
       c.xray_min_coupling_degree,
       c.x_ray_separate_overloads,
       c.use_parallel_code_health_strategy,
       c.scan_all_code_in_project,
       c.code_health_alert_level,
       rising.spottrendspan,
       s.status,
       c.selectdistinctcommits, c.ticketidpattern,
       c.minexdevcontribtime, c.exdevcontribenabled, c.temporal_coupling_strategy,
       c.temporal_coupling_across_minrevs, c.temporal_coupling_archlevel_across_minrevs,
       c.temporal_coupling_across_maxchangesetsize,
       c.systemleveltrend,
       c.parallelgitmining,
       c.pair_programming_pattern,
       c.pair_programming_author_message_pattern,
       c.combine_authors_field_and_commit_msg_to_deduce_pairs,
       c.modus_commit_message_pattern,
       c.complete_history_for_social,
       c.auto_detect_text_files, c.lookup_initial_file_size,
       c.hotspots_sliding_window_time,
       c.pm_data_sliding_window_time,
       c.team_analysis_sliding_window_time,
       c.team_analysis_start_date,
       c.age_is_last_commit,
       c.branch_delivery_risk_commit_limit,
       c.branch_name_exclusion_pattern,
       c.branches_analysis_lookback_weeks,
       c.calculate_branch_statistics,
       c.git_rename_limit,
       c.hotspots_defect_mining_pattern,
       c.code_ownership_source_file_names,
       c.disable_code_health_custom_rules,
       c.global_code_health_custom_rules,
       complexity.complexitytrendwarninglookbackinmonths,
       p.do_housekeeping, p.max_historic_age, p.max_history_depth,
       pg.name group_name,
       pg.id group_id
       from projects as p
       join project_analysis_configurations as c
            on p.config = c.id
       join analysis_status as s
            on p.id = s.project_id
       join project_complexity_trends_configuration as complexity
            on p.complexitytrend = complexity.id
       join project_rising_hotspots_configuration as rising
            on p.risingspots = rising.id
       left join project_groups_assignments as pga
            on p.id = pga.project_id
       left join project_groups as pg
            on pga.group_id = pg.id
       where p.id in (:project_ids)
       order by p.name asc;

-- name: foreign-project-config-ids
-- Returns the foreign keys of the project configrations.
 select config, complexitytrend, risingspots from projects as p
       where p.id = ?;

-- name: update-project!
-- Updates an existing project. Note that config is separate!
update projects
       set name = :name, analysisdest = :analysisdest, description = :description,
       project_owner = :project_owner, fixed_repo_roots = :fixed_repo_roots,
       code_comment_enabled = :code_comment_enabled, fixed_results_root = :fixed_results_root,
       ticket_id_uri_template = :ticket_id_uri_template, do_housekeeping = :do_housekeeping,
       max_historic_age = :max_historic_age, max_history_depth = :max_history_depth
       where id = ?;

-- name: update-complexity-trends-configuration!
-- Updates the configuration of complexity trends.
update project_complexity_trends_configuration
  set complexitytrendwarninglookbackinmonths = :complexitytrendwarninglookbackinmonths
       where id = ?;

-- name: update-rising-hotspots-configuration!
--- Updates the thresholds for rising spots warnings.
update project_rising_hotspots_configuration
  set spottrendspan = :spottrendspan
     where id = ?;

-- name: update-analysis-configuration!
-- Updates the analysis parameters for a project.
-- Note that some parameters are immutable. E.g. it doesn't make
-- sense to change VCS. In those cases, we want a new project instead.
update project_analysis_configurations
set
       firstanalysisdate = :firstanalysisdate, project_start_date = :project_start_date,
       minrevs = :minrevs, maxchangesetsize = :maxchangesetsize,
       exclusionfilter = :exclusionfilter, excludecontent = :excludecontent,
       whitelistcontent = :whitelistcontent, gitremotelocalpath = :gitremotelocalpath,
       delta_recommend_review_level = :delta_recommend_review_level,
       delta_always_comment = :delta_always_comment,
       delta_ignore_other_branches = :delta_ignore_other_branches,
       delta_coupling_threshold = :delta_coupling_threshold,
       delta_branch_name_exclusion_pattern = :delta_branch_name_exclusion_pattern,
       xrayallrevisions = :xrayallrevisions,
       xray_min_coupling_degree = :xray_min_coupling_degree,
       x_ray_separate_overloads = :x_ray_separate_overloads,
       use_parallel_code_health_strategy = :use_parallel_code_health_strategy,
       scan_all_code_in_project = :scan_all_code_in_project,
       code_health_alert_level = :code_health_alert_level,
       systemleveltrend = :systemleveltrend,
       parallelgitmining = :parallelgitmining,
       pair_programming_pattern = :pair_programming_pattern,
       pair_programming_author_message_pattern = :pair_programming_author_message_pattern,
       combine_authors_field_and_commit_msg_to_deduce_pairs = :combine_authors_field_and_commit_msg_to_deduce_pairs,
       pm_data_remove_outliers = :pm_data_remove_outliers,
       delivery_performance_strategy = :delivery_performance_strategy,
       delivery_performance_release_tag = :delivery_performance_release_tag,
       rollingaveragedays = :rollingaveragedays, update_repositories = :update_repositories, fail_on_update_repositories = :fail_on_update_repositories,
        selectdistinctcommits = :selectdistinctcommits, ticketidpattern = :ticketidpattern,
        minexdevcontribtime = :minexdevcontribtime, exdevcontribenabled = :exdevcontribenabled,
        temporal_coupling_strategy = :temporal_coupling_strategy,
        temporal_coupling_across_minrevs = :temporal_coupling_across_minrevs,
        temporal_coupling_archlevel_across_minrevs = :temporal_coupling_archlevel_across_minrevs,
        temporal_coupling_across_maxchangesetsize = :temporal_coupling_across_maxchangesetsize,
        modus_commit_message_pattern = :modus_commit_message_pattern,
        complete_history_for_social = :complete_history_for_social,
        auto_detect_text_files = :auto_detect_text_files, lookup_initial_file_size = :lookup_initial_file_size,
        hotspots_sliding_window_time = :hotspots_sliding_window_time,
        pm_data_sliding_window_time = :pm_data_sliding_window_time,
        team_analysis_sliding_window_time = :team_analysis_sliding_window_time,
        team_analysis_start_date = :team_analysis_start_date,
        age_is_last_commit = :age_is_last_commit,
        branch_delivery_risk_commit_limit = :branch_delivery_risk_commit_limit,
        branch_name_exclusion_pattern = :branch_name_exclusion_pattern,
        branches_analysis_lookback_weeks = :branches_analysis_lookback_weeks,
        calculate_branch_statistics = :calculate_branch_statistics,
        git_rename_limit = :git_rename_limit,
        hotspots_defect_mining_pattern = :hotspots_defect_mining_pattern,
        code_ownership_source_file_names = :code_ownership_source_file_names,
        disable_code_health_custom_rules = :disable_code_health_custom_rules,
        global_code_health_custom_rules = :global_code_health_custom_rules
       where id = ?;

-- name: update-analysis-destination!
-- Updates the path to analysis results.
update projects set analysisdest = :analysis_destination where id = :project_id;

-- name: update-remote-local-path!
-- Updates the path to cloned repos.
update project_analysis_configurations set gitremotelocalpath = :remote_local_path where id = :config_id;

-- name: insert-repository!
-- Inserts the paths to the repository paths associated with a project.
insert into project_repository_paths (path, project_id, branch)
values (:path, :project_id, :branch);

-- name: update-repository!
-- Updated repository by ID
update project_repository_paths set
project_id = :project_id, path = :path, branch = :branch where
id = :id;

-- name: insert-configured-project<!
-- Inserts a new project with a given configuration.
insert into projects
       (name, path, analysisdest,
       config, complexitytrend, risingspots, ticket_id_uri_template,
       do_housekeeping, max_historic_age, max_history_depth, project_owner,
       fixed_repo_roots,
       fixed_results_root,
       code_comment_enabled,
       created_at)
       values
       (:name, :path, :analysisdest,
       :config, :complexitytrend, :risingspots, :ticket_id_uri_template,
       :do_housekeeping, :max_historic_age, :max_history_depth, :project_owner,
       :fixed_repo_roots,
       :fixed_results_root,
       :code_comment_enabled,
       :created_at);

-- name: insert-complexity-trends-configuration<!
-- Inserts a configuration describing the parameters to use for complexity trend calculations.
insert into project_complexity_trends_configuration
       (complexitytrendwarninglookbackinmonths)
       values(:complexitytrendwarninglookbackinmonths
);

-- name: insert-rising-hotspots-configuration<!
insert into project_rising_hotspots_configuration
       (minspotrevisions, minspotcodesize, minnewspotrank, minrankincrease, spottrendspan)
       values(:minspotrevisions, :minspotcodesize, :minnewspotrank, :minrankincrease, :spottrendspan);

-- name: insert-analysis-configuration<!
-- Inserts a configuration describing the analysis parameters to use.
insert into project_analysis_configurations
       (vcs, project_start_date, firstanalysisdate, analysisplan,
       minrevs, maxchangesetsize, exclusionfilter, excludecontent, whitelistcontent,
       gitremotelocalpath,
       delta_recommend_review_level,
       delta_always_comment,
       delta_ignore_other_branches,
       delta_coupling_threshold,
       delta_branch_name_exclusion_pattern,
       xrayallrevisions,
       xray_min_coupling_degree,
       x_ray_separate_overloads,
       use_parallel_code_health_strategy,
       scan_all_code_in_project,
       code_health_alert_level,
       systemleveltrend,
       parallelgitmining,
       pair_programming_pattern,
       pair_programming_author_message_pattern,
       combine_authors_field_and_commit_msg_to_deduce_pairs,
       pm_data_remove_outliers,
       delivery_performance_strategy,
       delivery_performance_release_tag,
       rollingaveragedays, update_repositories, fail_on_update_repositories,
       selectdistinctcommits, ticketidpattern, minexdevcontribtime, exdevcontribenabled,
       temporal_coupling_strategy,
       temporal_coupling_across_minrevs, temporal_coupling_archlevel_across_minrevs,
       temporal_coupling_across_maxchangesetsize,
       modus_commit_message_pattern,
       complete_history_for_social,
       auto_detect_text_files,
       lookup_initial_file_size,
       hotspots_sliding_window_time,
       pm_data_sliding_window_time,
       team_analysis_sliding_window_time,
       team_analysis_start_date,
       age_is_last_commit,
       branch_delivery_risk_commit_limit,
       branch_name_exclusion_pattern,
       branches_analysis_lookback_weeks,
       calculate_branch_statistics,
       git_rename_limit,
       hotspots_defect_mining_pattern,
       code_ownership_source_file_names,
       disable_code_health_custom_rules,
       global_code_health_custom_rules)
       values(:vcs, :project_start_date, :firstanalysisdate, :analysisplan,
       :minrevs, :maxchangesetsize, :exclusionfilter, :excludecontent, :whitelistcontent,
       :gitremotelocalpath,
       :delta_recommend_review_level,
       :delta_always_comment,
       :delta_ignore_other_branches,
       :delta_coupling_threshold,
       :delta_branch_name_exclusion_pattern,
       :xrayallrevisions,
       :xray_min_coupling_degree,
       :x_ray_separate_overloads,
       :use_parallel_code_health_strategy,
       :scan_all_code_in_project,
       :code_health_alert_level,
       :systemleveltrend,
       :parallelgitmining,
       :pair_programming_pattern,
       :pair_programming_author_message_pattern,
       :combine_authors_field_and_commit_msg_to_deduce_pairs,
       :pm_data_remove_outliers,
       :delivery_performance_strategy,
       :delivery_performance_release_tag,
       :rollingaveragedays, :update_repositories, :fail_on_update_repositories,
       :selectdistinctcommits, :ticketidpattern, :minexdevcontribtime, :exdevcontribenabled,
       :temporal_coupling_strategy,
       :temporal_coupling_across_minrevs, :temporal_coupling_archlevel_across_minrevs,
       :temporal_coupling_across_maxchangesetsize,
       :modus_commit_message_pattern,
       :complete_history_for_social,
       :auto_detect_text_files,
       :lookup_initial_file_size,
       :hotspots_sliding_window_time,
       :pm_data_sliding_window_time,
       :team_analysis_sliding_window_time,
       :team_analysis_start_date,
       :age_is_last_commit,
       :branch_delivery_risk_commit_limit,
       :branch_name_exclusion_pattern,
       :branches_analysis_lookback_weeks,
       :calculate_branch_statistics,
       :git_rename_limit,
       :hotspots_defect_mining_pattern,
       :code_ownership_source_file_names,
       :disable_code_health_custom_rules,
       :global_code_health_custom_rules
);

-- name: insert-analysis-status-for-new-project!
-- Inserts the status of a new project.
insert into analysis_status
       (status, last_status_change, project_id)
       values(0, CURRENT_TIMESTAMP(), :project_id);

-- name: project-temporal-coupling-filters
select project_id, name, pattern_1, pattern_2
       from project_temporal_coupling_filters
       where project_id = :project_id;

-- name: delete-all-project-temporal-coupling-filters!
delete from project_temporal_coupling_filters;

-- name: delete-project-temporal-coupling-filters!
delete from project_temporal_coupling_filters
       where project_id = :project_id;

-- name: insert-project-temporal-coupling-filter<!
insert into project_temporal_coupling_filters (project_id, name, pattern_1, pattern_2)
       values (:project_id, :name, :pattern_1, :pattern_2);

-- name: get-commits-to-exclude
-- Returns the commit hashes to exclude from the analysis.
select hash from project_exclude_commits as p
       where p.project_id = ?;

-- name: insert-commits-to-exclude!
insert into project_exclude_commits (project_id, hash)
       values (:project_id, :hash);

-- name: delete-commits-to-exclude!
delete from project_exclude_commits where project_id = :project_id;