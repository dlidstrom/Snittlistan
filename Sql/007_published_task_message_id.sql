ALTER TABLE snittlistan.published_task ALTER message_id SET NOT NULL;
ALTER TABLE snittlistan.published_task ADD UNIQUE (message_id);
