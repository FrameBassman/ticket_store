DO
$do$
BEGIN
   IF NOT EXISTS (
      SELECT
      FROM   pg_catalog.pg_roles
      WHERE  rolname = 'store_user') THEN

      CREATE ROLE store_user LOGIN PASSWORD 'KqCQzyH2akGB9gQ4';
      GRANT SELECT ON ALL TABLES IN SCHEMA public TO store_user;
   END IF;
END
$do$;

CREATE EXTENSION pgcrypto;