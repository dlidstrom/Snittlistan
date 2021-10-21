-- index any foreign keys!
CREATE TABLE delayed_task (
    delayed_task_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    business_key VARCHAR(255) NOT NULL,
    data JSONB NOT NULL,
    publish_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    published_date TIMESTAMP WITHOUT TIME ZONE NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL
);
