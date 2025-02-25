-- DEFAULT VARIABLE TYPES
IF NOT EXISTS(SELECT 1 FROM variable_type)
BEGIN
	INSERT INTO variable_type([name],created_at_utc,modified_at_utc,is_deleted,deleted_at_utc)
	VALUES ('string',GETUTCDATE(),GETUTCDATE(),0,CONVERT(datetime2,'1753-1-1')),
		   ('number',GETUTCDATE(),GETUTCDATE(),0,CONVERT(datetime2,'1753-1-1')),
		   ('float',GETUTCDATE(),GETUTCDATE(),0,CONVERT(datetime2,'1753-1-1')),
		   ('double',GETUTCDATE(),GETUTCDATE(),0,CONVERT(datetime2,'1753-1-1'))

	PRINT 'DEFAULT VARIABLES TYPES SAVED'
END

-- DEFAULT VARIABLES
IF NOT EXISTS(SELECT 1 FROM variable)
BEGIN
	DECLARE @STRING_ID INT = (SELECT Id FROM variable_type WHERE [name] = 'string')

	INSERT INTO variable([name],[description],variable_type_id,created_at_utc,modified_at_utc,is_deleted,deleted_at_utc)
	VALUES
		('FirstName','Represent the firstname of the user',@STRING_ID,GETUTCDATE(),GETUTCDATE(),0,CONVERT(datetime2,'1753-1-1')),
		('LastName','Represent the lastname of the user',@STRING_ID,GETUTCDATE(),GETUTCDATE(),0,CONVERT(datetime2,'1753-1-1')),
		('Email','Represent the email of the user',@STRING_ID,GETUTCDATE(),GETUTCDATE(),0,CONVERT(datetime2,'1753-1-1')),
		('EmailCode','Represent the email code that is going to be send to the user',@STRING_ID,GETUTCDATE(),GETUTCDATE(),0,CONVERT(datetime2,'1753-1-1'));

    PRINT 'DEFAULT VARIABLES SAVED';
END

-- DEFAULT TEMPLATE CATEGORIES
IF NOT EXISTS(SELECT 1 FROM template_category)
BEGIN
	INSERT INTO template_category([name],[description],created_at_utc,modified_at_utc,is_deleted,deleted_at_utc)
	VALUES
		('email_code','Represent the email code notification template category',GETUTCDATE(),GETUTCDATE(),0,CONVERT(datetime2,'1753-1-1'));

	PRINT 'DEFAULT TEMPLATE CATEGORIES SAVED';
END

-- DEFAULT TEMPLATES
IF NOT EXISTS(SELECT 1 FROM template)
BEGIN
	DECLARE @EMAIL_CODE_ID INT = (SELECT TOP 1 id FROM template_category WHERE [name] = 'email_code')

	INSERT INTO template([title],content,template_category_id,is_active,is_default,is_deleted,created_at_utc,modified_at_utc,deleted_at_utc)
	VALUES
		('Email Verification Request','<html><head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"><title>Verification Code</title><style>body{font-family:Arial,sans-serif;background-color:#f4f4f4;text-align:center;padding:20px}.container{max-width:500px;background:#fff;padding:20px;border-radius:8px;box-shadow:0 0 10px rgba(0,0,0,.1);margin:auto}.code{font-size:24px;font-weight:700;color:#333;background:#f8f8f8;padding:10px;display:inline-block;border-radius:5px;margin:20px 0}.footer{font-size:12px;color:#777;margin-top:20px}</style></head><body><div class="container"><h2>Email Verification Request</h2><p>We received a request to verify your email address (<strong>{{ Email }}</strong>).</p><p>Use the following verification code to complete the process:</p><div class="code">{{ EmailCode }}</div><p>If you did not request this, you can ignore this email.</p><p>Thank you,<br>The Team</p><p class="footer">This is an automated message. Please do not reply.</p></div></body></html>',@EMAIL_CODE_ID,0,1,0,GETUTCDATE(),GETUTCDATE(),CONVERT(datetime2,'1753-1-1'))

	PRINT 'DEFAULT TEMPLATES SAVED'
END

-- DEFAULT TEMPLATE VARIABLES 
IF NOT EXISTS(SELECT 1 FROM template_variable)
BEGIN
	-- EMAIL CODE TEMPLATE
	DECLARE @EMAIL_CODE_TEMPLATE_ID INT = (SELECT TOP 1 A.id FROM template A INNER JOIN template_category B ON A.template_category_id = B.id WHERE B.[name] = 'email_code');

	INSERT INTO template_variable(template_id, variable_id, created_at_utc, modified_at_utc)
	SELECT @EMAIL_CODE_TEMPLATE_ID, Id, GETUTCDATE(), GETUTCDATE()
	FROM variable
	WHERE [name] IN ('FirstName', 'LastName', 'Email', 'EmailCode')

	PRINT 'DEFAULT TEMPLATE VARIABLES SAVED'
END