-- index any foreign keys!
CREATE TABLE snittlistan.change_log (
    change_log_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    tenant_id INT NOT NULL REFERENCES snittlistan.tenant,
    correlation_id UUID NOT NULL,
    causation_id UUID NULL,
    command_type VARCHAR(1024) NOT NULL,
    data JSONB NOT NULL,
    created_by VARCHAR(255) NOT NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);
