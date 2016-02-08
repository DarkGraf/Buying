-- ***************************************************
-- Уменьшение длины поля Description таблицы Comments.
-- ***************************************************

declare @lenCommentsDescription int
select @lenCommentsDescription = CHARACTER_MAXIMUM_LENGTH 
from INFORMATION_SCHEMA.COLUMNS 
where TABLE_NAME = 'Comments' and COLUMN_NAME = 'Description'

if @lenCommentsDescription = 200
begin
  update Comments
  set Description = substring(Description, 1, 50)
  alter table Comments alter column Description nchar(50)
  print 'Уменьшение длины поля Description таблицы Comments.'
end

-- ***************************************************
-- Установка для поля Id таблицы Comments значения
-- по уполчанию.
-- ***************************************************
if (select object_definition(default_object_id) as definition
   from sys.columns where name = 'Id' and object_id = object_id('Comments')) is null
begin
  alter table Comments add default newid() for Id
  print 'Установка для поля Id таблицы Comments значения по уполчанию.'
end

-- ***************************************************
-- Установка для поля Id таблицы Goods значения
-- по уполчанию.
-- ***************************************************
if (select object_definition(default_object_id) as definition
   from sys.columns where name = 'Id' and object_id = object_id('Goods')) is null
begin
  alter table Goods add default newid() for Id
  print 'Установка для поля Id таблицы Goods значения по уполчанию.'
end