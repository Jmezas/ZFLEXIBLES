select * from LOG_Material



DBCC CHECKIDENT (LOG_Material, RESEED,0)


insert into LOG_Material(sNomMat,nPrecioCompra,nPrecioVenta)
			values('TORTIMAS 250 GR.',498.394,721.812),
				('INKALIMENTOS HARINA DE MAIZ/QUINUA/KIWICHA',790.85175,1145.3715),
				('INKALIMENTOS HARINA DE HABA/CEBADA/MEZCLA',795.9195,1152.711),
				('INKALIMENTOS HOJUELA QUINUA/KIWICHA',633.59925,917.6265),
				('INKALIMENTOS HOJUELA AVENA PRECOCIDA/QUINUA/KIWICHA',1025.56325,1485.2985),
				('SKY FRUTADO CON FRESA',321.93625,499.55625),
				('TURQUINO',557.28575,882.75),	
				('HELADO GOLOSO',364.3125,583.31),
				('HELADO CHARLES CHOCOLATE',636.86175,1006.23),
				('SUPREMO CHOCOLATE',421.167,671.54),
				('SHERVET  DE FRESA UNA BANDA',167.04,277.2),
				('LIVLY',24.273,37.665),
				('WARNING',9.251,14.355),
				('ESPINACA DARIO',293.2335,455.0175),
				('GELATINA LITORAL 5 KG.',193.401,300.105),
				('POLIMERO',10.179,15.795),
				('TOFFE CAF�',467.0015,724.6575),
				('TOFFE PREMIUN',579.09375,898.59375),
				('CAMISAS G. Y. L',30.015,46.575),
				('FABRIPACK 10 X 15 INSERTO 11 X 16',180.409,279.945),
				('FABRIPACK 13 X 19',167.04,259.2),
				('FABRIPACK 9 x 14',125.86,195.3),
				('PAPITAS AL HILO',132.704,205.92),
				('SUPER CHISMOCITO 16 X 19',83.52,129.6),
				('SUPER CHISMOSITO 21X24',61.103,94.815),
				('BOLSA RELY 12 X 16',187.862,291.51),
				('BOLSA MAYER 12 X 16',187.862,291.51),
				('BOLSA YANDAO'	,17.9655,27.8775),
				('BOLSA SOL PRAIA AZUL',9.4685,14.6925),
				('CALIBRACION ZAVALA ',0.00,0.00),				
				('AZUCAR FINITA ',29.68875,46.06875),
				('DETERGENTE BOLIVAR 800 G.',193.7055,300.5775),
				('FONDO JARABE DE GOMA',102.863,159.615),
				('INSERTO COCONA KRIM 12 GR',3.915,6.075),
				('INSERTO HIELO SKYICE 2 KG',0.638,0.99),
				('PAN DE MOLDE INTEGRAL EL ALGARROBO ( CAMBIA NOMBRE )',0.00,86.4),
				('SAL VIVA 900 gr.',215.325,334.125),
				('TACAMA 14 X 20',64.96,100.8),
				('TACAMA 15 X 25',64.38,99.9),
				('VINIFAN 100 Y 50 UND ( OTRO NOMBRE )',375.1875,582.1875),
				('DETERGENTE MARCELLA 350 G.',303.60825,471.11625),
				('INSERTO GELATINA PERFECTA',0.00,0.00),
				('INSERTO MARISAL COCINA 1 KG.',11.745,18.225),
				('INSERTO SAL GUILLEN ',3.625,5.625),
				('INSERTO SAL GUILLEN 2',2.436,3.78),
				('AGREMAX CONCRETO',178.5965,277.1325),
				('AGREMAX TERRAJEO',123.4095,191.4975),
				('FULL CONCRETOS 175',109.81575,170.40375) ,
				('OFERTON BLANCO FLEXIBLE',47.966,74.43),
				('SHOTCRETE VERDE',89.958,139.59),
				('OFERTON BLANCO FLEXIBLE (NEGRO)',7.0035,10.8675),
				('ANGEL ROLLO ',45.1095,69.9975),
				('INSERTO 21 X 24',1.595,2.475),
				('PAMER 16X19',201.376,312.48),
				('INVERSIONES SAN ANTONIO 16 X 19 CANARIO',206.973,321.165),
				('INVERSIONES SAN ANTONIO 16 X 19 PIOLIN',181.888,282.24),
				('HIELO EN CUBO',20.01,31.05),
				('GELATINA OKEY FRESA',661.2,957.6),
				('GELATINA OKEY PI�A NARANJA',604.563,875.574),
				('BIOMACA KIDS ARITOS',416.498,646.29),
				('ROMANCE',332.97075,516.67875),
				('HORIONG',36.16,56.115),
				('BRILLOMIX 12 X 18',100.688,156.24),
				('BRILLOMIX 13 X 19 / 14 X 20',102.515,159.075),		
				('DBRILLO 11 X 16',77.952,120.96),
				('DBRILLO 12 X 18',103.936,161.28),
				('DBRILLO 13 X 19 / 14 X 20',105.241,163.305),		
				('DBRILLO 9 X 14 / 10 X 15',66.961,103.905),
				('SUPER GORRIONCITO 16 X 19',89.088,138.24),
				('SUPER GORROCIONCITO 16 X 19',89.09,138.24),
				('LA COLOCHA 16 X 19',154.744,240.12),
				('CABLES DE TELECOMUNICACIONES',48.2125,74.8125),
				('CINTA GEOCEAN PERU',25.839,40.095),
				('CINTA PELIGRO SEDAPAL',52.258,81.09),
				('JOLIE',227.23675,352.60875),
				('CINTA NO EXCAVAR',58.73,91.125),	
				('CINTA PELIGRO NO PASAR',189.805,294.525),
				('ARROZ EXTRA A�EJO 5 KG',526.988,763.224),
				('BANANO BIO NATURE',436.21075,631.7535),
				('COCOA LA NIETA',986.696,1429.008),
				('INCATOM VERDE',115.9275,167.895),
				('BIO NATURE ACTIVE',79.7355,115.479),
				('BANANEN BIO IREN',220.806,319.788),
				('APPFONORPE',38.69,60.03),
				('NEGOCIACIONES YOSELIN SAC',22.0545,34.2225),
				('CONCRELISTO',132.59,205.74),
				('NETA 20/40 ',361.456,560.88),
				('CHOCOLATE MARQUINO',686.517,1065.285),
				('ARRROZ EXTRA SUPERIOR 5 KG',68.904,106.92),
				('AZUCAR EXTRA SUPERIOR 5 KG',67.86,105.3),
				('CRIS VIDA',184.33125,286.03125),
				('HIELO SAN FRANCISCO 3 KG.',22.4895,34.8975),
				('GELATINA MONTENEGRO 5KG.',193.26325,299.89125),
				('FINGER ITPM ',392.08,608.4),
				('FINGER ITPM NUEVO',301.716,468.18),
				('HELADOS SABORATTI OK CYAN',64.554,0.00),
				('INSERTO PIE DE IMPRENTA ',0.609,0.945),
				('INSERTO QUIKRETE CONCRELISTO',7.337,11.385),
				('RETIRA QUIKRETE CONCRELISTO',28.652,44.46),
				('PRUEBA ARO',19.7925,30.7125),
				('PRUEBA ARO CONDIMENTOS Y ESPECIAS',5.742,8.91),
				('AZUL CRIS VIDA',50.025,77.625),
				('GELATINA OKEY FRESA FONDO',267.786,415.53),
				('GELATINA OKEY PI�A NARANJA FONDO',139.5625,216.5625),
				('OFERTON BLANCO FLEXIBLE (NARANJA)',7.3515,11.4075)
