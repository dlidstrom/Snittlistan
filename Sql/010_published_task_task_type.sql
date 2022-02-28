-- only allow one unhandled published_task for each task_type at a time
ALTER TABLE snittlistan.published_task ADD task_type INT NULL;
CREATE UNIQUE INDEX published_task_task_type_idx ON
snittlistan.published_task(task_type)
WHERE
handled_date IS NULL;
