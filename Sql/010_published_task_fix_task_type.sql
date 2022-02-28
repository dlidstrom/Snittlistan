DROP INDEX IF EXISTS snittlistan.published_task_task_type_idx;
ALTER TABLE snittlistan.published_task ALTER task_type SET NOT NULL;
CREATE UNIQUE INDEX published_task_task_type_idx ON
snittlistan.published_task(tenant_id, task_type)
WHERE
handled_date IS NULL;
