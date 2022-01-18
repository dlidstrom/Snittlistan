-- index any foreign keys!
CREATE TABLE snittlistan.rate_limit (
    rate_limit_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    key varchar(100) NOT NULL UNIQUE, -- for example, user-1:logon_mail
    allowance DECIMAL NOT NULL, -- allowance, will be max `rate - 1`
    rate INT NOT NULL, -- readonly, it is the `burst` value, i.e. how many are allowed to be sent `per_seconds`
    per_seconds INT NOT NULL,
    updated_date TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);
