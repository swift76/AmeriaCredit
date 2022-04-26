insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('USER_PASSWORD_EXPIRY', '90', N'Օգտագործողի գաղտնաբառի ժամկետ /օր/')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('CREDIT_CARD_AUTH_TERM', '1800', N'Պլաստիկ քարտի նույնականացման կոդի վավերականության ժամկետ /վրկ/')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('CREDIT_CARD_TRY_COUNT', '5', N'Պլաստիկ քարտի վավերացման անհաջող փորձերի քանակ')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('CREDIT_CARD_SMS_COUNT', '3', N'Պլաստիկ քարտի վավերացման համար ուղարկվող հաղորդագրությունների քանակ')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('BANK_SERVER_DATABASE', '[(LOCAL)].bank.', N'Բանկային պահոց')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('SEND_SERVER_DATABASE', '', N'SMS/Email ուղարկելու պահոց')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('FILE_MAX_SIZE', '512000', N'Վերբեռնվող ֆայլի առավելագույն չափ')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('NORQ_TRY_COUNT', '5', N'NORQ հարցումների փորձերի քանակ')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('ACRA_TRY_COUNT', '5', N'ACRA հարցումների փորձերի քանակ')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('NORQ_CACHE_DAY', '0', N'NORQ-ի հարցումների կրկին օգտագործման ժամկետ /օր/')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('ACRA_CACHE_DAY', '0', N'ACRA-ի հարցումների կրկին օգտագործման ժամկետ /օր/')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('MOBILE_PHONE_AUTH_TERM', '1800', N'Բջջային հեռախոսի նույնականացման կոդի վավերականության ժամկետ /վրկ/')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('MOBILE_PHONE_SMS_COUNT', '3', N'Բջջային հեռախոսի վավերացման համար ուղարկվող հաղորդագրությունների քանակ')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('MOBILE_PHONE_SMS_CHECK', '0', N'Բջջային հեռախոսի նույնականացման կոդի ստուգում')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('IS_MONITORING', '0', N'Մոնիտորինգ եղանակով է')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('REDIRECT_TO_ONBOARDING', '0', N'Անցում Onboarding')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('OFFLINE_AUTHENTICATION', '0', N'Ոչ առցանց նույնականացում')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('EMAIL_VERIFICATION', '0', N'Էլ. փոստի հաստատում')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('EVENTSTORE_SERVER_DATABASE', 'EventStore.', N'Event Store պահոց')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('AUTHORIZATION_CODE_TRY_COUNT', '5', N'Նույնականացման կոդի վավերացման անհաջող փորձերի քանակ')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('CALL_BACK_URL', 'https://customer.ameriabank.am/', N'Վերադարձի հղում')
GO
insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('AGREEMENT_LIMIT', '300000', N'Համաձայնագրի սահմանաչափ')
GO
