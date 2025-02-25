-- Author: Sebastian Vargas (Szxro)
-- Description: Set a desired user by its id to be an admin

-- NOTE: NEED TO BE CHANGE TO THE USER ID THAT YOU WANT TO BE AN ADMIN
DECLARE @USER_ID INT = 0;

UPDATE [user] 
SET is_admin = 1
WHERE id = @USER_ID

