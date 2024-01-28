DROP TABLE IF EXISTS actors;
DROP TABLE IF EXISTS movies;

CREATE TABLE actors
(
    id         UUID PRIMARY KEY,
    first_name VARCHAR(255) NOT NULL,
    last_name  VARCHAR(255) NOT NULL,
    birthday   DATE         NOT NULL
);

CREATE TABLE movies
(
    id          UUID PRIMARY KEY,
    title       VARCHAR(255) NOT NULL,
    description TEXT         NOT NULL,
    release     DATE         NOT NULL
);

CREATE INDEX movies_title_idx ON movies (title);

CREATE TABLE actors_movies
(
    id       int GENERATED ALWAYS AS IDENTITY,
    fk_actor UUID NOT NULL,
    fk_movie UUID NOT NULL,
    PRIMARY KEY (id)
);

ALTER TABLE actors_movies
    ADD CONSTRAINT fkc_actors_actors_movies FOREIGN KEY (fk_actor) REFERENCES actors (id)
        ON DELETE CASCADE
        ON UPDATE CASCADE;

ALTER TABLE actors_movies
    ADD CONSTRAINT fkc_movies_actors_movies FOREIGN KEY (fk_movie) REFERENCES movies (id)
        ON DELETE CASCADE
        ON UPDATE CASCADE;