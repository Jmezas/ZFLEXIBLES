
select * from LOG_Cliente
delete from LOG_Cliente
DBCC CHECKIDENT (LOG_Cliente, RESEED,0)

insert into LOG_Cliente(sNroDoc,sRazonSocial,sDireccion,sEmail)
				values ('20601801371','AGROINDUSTRIA LEGASA S.A.C','BOSQUE Nº F-4 FUNDO TANCARPAMPA CUSCO - ANTA - PUCYURA','zflexibles1@gmail.com'),
						('20510459980','AV Y F ASOCIADOS S.A.C.','MZA. C LOTE. 10 ASOC. VIVI. VILLA VITARTE LIMA - LIMA - ATE','tesoreria@saboratti.com'),
						('20603321724','CORPORACION PLASTICA BRAVO E.I.R.L.','CAL.5 MZA. G LOTE. 4 URB. BARBADILLO LIMA - LIMA - ATE','zflexibles1@gmail.com'),
						('','CESAR ORTIZ','','zflexibles1@gmail.com'),
						('','DANIEL AQUINO','','zflexibles1@gmail.com'),
						('','DENIS PANDO','','zflexibles1@gmail.com'),
						('20554293167','DULCINA DEL PERU S.A.C.','JR. SAYHUITE NRO. 174 URB. ZARATE LIMA - LIMA - SAN JUAN DE LURIGANCHO','zflexibles1@gmail.com'),
						('','EDDY PRETEL','','zflexibles1@gmail.com'),
						('','FABRIPACK','','zflexibles1@gmail.com'),
						('','FIDEL VILCA','','zflexibles1@gmail.com'),
						('20535862959','FLEXOPACK PERU S.A.C.','AV. INTIHUATANA NRO. 654 (URB. EL PEDREGAL) LIMA - LIMA - SURQUILLO','zflexibles1@gmail.com'),
						('','GERARDO','','zflexibles1@gmail.com'),
						('20544767284','GRAFI SOLUTION S.A.C.','CAL.SANTA COLETA DE CORBIE NRO. 172 URB. PANDO 3RA ET  LIMA - LIMA - LIMA','zflexibles1@gmail.com'),
						('20537079631',' HINVERPLAST S.R.L.','BLOCK D MZA. A LOTE. 03 DPTO. 503 PARCELA SEMIRUSTICA ZAVALETA LIMA - LIMA - ATE','zflexibles1@gmail.com'),
						('20603334842','HIELOS PERUANOS E.I.R.L.','JR. REPUBLICA DE MEXICO NRO.143 VLL. SEÑOR DE LOS MILAGROS-PROV.CONST.DEL CALLAO-CALLAO','zflexibles1@gmail.com'),
						('20100271282','INDUSTRIAS ZCHSS S R LTDA','AV. PLACIDO JIMENEZ NRO. 989 (CEMENTERIO PADRE ETERNO Y FUNDICION LIMA) LIMA - LIMA - LIMA','zflexibles1@gmail.com'),
						('20382757981','INDUSTRIAS PLASTICAS CAUTE S.R.L.','CAL.VICTOR REYNEL NRO. 760 (CUADRA 20 AV. ARGENTINA) LIMA - LIMA - LIMA','zflexibles1@gmail.com'),
						('','LARICO','','zflexibles1@gmail.com'),
						('','MARCIAL ROCA','','zflexibles1@gmail.com'),
						('','PELAYO','','zflexibles1@gmail.com'),
						('','PIERO','','zflexibles1@gmail.com'),
						('20537009188','POLIEMPAQUES INDUSTRIALES S.A.C.','MZA. D LOTE. 7 URB. JARDIN AZUL LIMA - LIMA - ATE','zflexibles1@gmail.com'),
						('20103510652','POLISA S.R.LTDA','CAL.YAHUAR HUACA NRO. 178 LAMBAYEQUE - CHICLAYO - LA VICTORIA','zflexibles1@gmail.com'),
						('20484194026','POLYBAGS PERU S.R.L.','AV. LURIGANCHO 1274 Z.I. ZARATE - SAN JUAN DE LURIGANCHO - LIMA - LIMA','logistica@polybagsperu.com'),
						('20484194026','POLYBAGS PERU S.R.L.','MZ. 35 A LOTE 1-2 CHOSICA DEL NORTE - CHICLAYO','dvega@polybagsperu.com'),
						('20340774362','QUIKRETE PERU S.A.',' AV. TOMAS MARSANO NRO. 2813 INT. 407 LIMA - LIMA - SANTIAGO DE SURCO','zflexibles1@gmail.com'),
						('','SHIRLEY PLAST','','zflexibles1@gmail.com'),
						('20101935168','TRANSFORMACIONES QUIMICAS DEL PERU S.A.C','AV. LOS PROCERES NRO. 125 LIMA - LIMA - RIMAC','zflexibles1@gmail.com'),
						('','YURI MORALES ','','zflexibles1@gmail.com'),
						('20537804743','ZFLEXIBLES S.A.C','AV. SEPARADORA INDUSTRIAL NRO. 979 A.H. SICUANI (PI 1 ALT.PARQ LOS ANILLOS) LIMA - LIMA - ATE','zflexibles1@gmail.com')


