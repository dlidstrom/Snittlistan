-- index any foreign keys!
CREATE TABLE snittlistan.roster_mail (
    roster_mail_id INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    roster_id varchar(20) NOT NULL,
    published_date TIMESTAMP WITHOUT TIME ZONE NULL, -- when it was published
    created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);
-- only allow one unpublished roster_mail for each roster at a time
CREATE INDEX roster_mail_idx ON
snittlistan.roster_mail(roster_id)
WHERE
published_date IS NULL;
