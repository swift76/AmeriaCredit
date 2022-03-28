if exists (select * from sys.objects where name='SHOP' and type='U')
	drop table IL.SHOP
GO

CREATE TABLE IL.SHOP (
	CODE			char(4)			NOT NULL,
	NAME			nvarchar(50)	NOT NULL,
	NAME_EN			varchar(50)		NOT NULL,
	HEAD_CODE		char(4)			NULL,
	ADDRESS			nvarchar(100)	NULL,
	ADDRESS_EN		varchar(100)	NULL,
	IS_DELIVERY		bit				NOT NULL,
	SEQUENCE_NUMBER int				NOT NULL default 1
)
GO

CREATE UNIQUE CLUSTERED INDEX iSHOP1 ON IL.SHOP(CODE)
GO
