Insert into Threads (Title, Description) values ('kakashka', 'Test kakashki ');

select * from Threads

select * from Users

Insert into Posts (Title, Content, AuthorId, ThreadId) Values ('Testovyi post', 'Eto testovyi post -_-', 1, 1); 

select * from Posts;

INSERT INTO Comments (Content, PostId, AuthorID) Values ('Moriarti', 2, 1);

SELECT * FROM Threads WHERE Id = 1;

SELECT * 
FROM Posts
WHERE ThreadId = 1;


Select * from Comments;

ALTER TABLE Comments
DROP CONSTRAINT FK_Comments_Users_AuthorId;

ALTER TABLE Comments
ADD CONSTRAINT FK_Comments_Users_AuthorId
FOREIGN KEY (AuthorId) REFERENCES Users(Id)
ON DELETE NO ACTION;

SELECT name
FROM sys.foreign_keys
WHERE parent_object_id = OBJECT_ID('Comments');

SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Comments';

ALTER TABLE Comments
DROP COLUMN AuthorId;
