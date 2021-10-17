Action<int, string> print = (n, msg) => bc.printLCD(n, msg);
Action<int> delay = (n) => bc.wait(n);
Action<int, int> motor = (left, right) => bc.onTF(left, right);
Action<int, float> rot = (speed, angle) => bc.onTFRot(speed, angle);

Func<int, string> cor = (sensor) => bc.returnColor(sensor);
Func<int, float> luz = (sensor) => bc.lightness(sensor - 1);
Func<int, float> corR = (sensor) => bc.returnRed(sensor);
Func<int, float> corG = (sensor) => bc.returnGreen(sensor);
Func<int, float> corB = (sensor) => bc.returnBlue(sensor);

int velocidade = 150;
int distancia = 220;
int distancia2 = 230;
int cronometro = 3000;
int Sensibilidade = 18;

float currentTime = 0;
float black = 40; //MUDAR CONFORME HORARIO 

bool resgateVal = false;
bool val1 = false;
bool val2 = false;
bool quadrado = false;

//sonarUP = bc.distance(0);
//sonarLEFT = bc.distance(1);
//sonarDOWN = bc.distance(2);

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Action<int, int> descer = (a, b) =>{
    bc.actuatorSpeed(a);
    bc.actuatorDown(b);
};

Action<int, int> subir = (a, b) =>{
    bc.actuatorSpeed(a);
    bc.actuatorUp(b);
};

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Action achouBolinha2 = () =>{
    {//De Frente pra VITIMA

        motor(-300, -300);
        delay(50);
        {
            if (bc.compass() > 0 && bc.compass() < 45)// NORTE direita 
            {
                print(1, "NORTE ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 0 && bc.compass() < 90);
            }

            else if (bc.compass() > 315 && bc.compass() < 360)// NORTE esquerda
            {
                print(1, "NORTE ATIVADO");
                bc.onTFRot(1000, 45);
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 0 && bc.compass() < 90);
            }

            else if (bc.compass() > 225 && bc.compass() < 315)// OESTE
            {
                print(1, "OESTE ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 225 && bc.compass() < 360);
            }

            else if (bc.compass() > 135 && bc.compass() < 225)// SUL
            {
                print(1, "SUL ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 135 && bc.compass() < 270);
            }

            else if (bc.compass() > 45 && bc.compass() < 135)// LESTE
            {
                print(1, "LESTE ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 45 && bc.compass() < 180);
            }
            motor(0, 0);
        }
    }

    {//Descer o Atuador
        descer(150, 600);
        bc.turnActuatorDown(100);
    }

    {//Pegar a VITIMA
        currentTime = bc.millis();
        while (!bc.hasVictims() && (bc.millis() - currentTime) <= 3000)
        {
            motor(300, 300);
        }
    }

    {//encaixar VITIMA
        motor(1000, 1000);
        delay(700);
        subir(90, 900);
    }

    //Verificar VITIMA
    if (!bc.hasVictims())
    {
        print(1, "NAO TENHO");
        while (!bc.touch(0))
        {
            motor(-1000, -1000);
        }
        bc.onTFRot(500, -90);
        
        while(bc.distance(2) < 20){
            motor(1000,1000);
        }

        motor(-300,-300);
        delay(1200);

        bc.resetTimer();
        goto terminar;

    }
    else if (bc.hasVictims())
    {
        print(1, "TENHO");

        motor(0, 0);
        delay(500);           // 800
        motor(100, 100);     // 70 70
        delay(500);         // 800
        motor(-70, -70);   // 50 50
        delay(800);

        if (!bc.hasVictims())
        {
            print(1, "NAO TENHO");
            while (!bc.touch(0))
            {
                motor(-1000, -1000);
            }

            bc.onTFRot(500, -90);
            
            while(luz(6) > 13)
            {
                motor(1000, 1000);    
            }
        
            bc.resetTimer();
            goto terminar;
        }

        //FICAR RETO PRA PAREDE/RESGATE
        bc.turnActuatorDown(400);
        bc.onTFRot(500, -90);

        //CHEGAR NA PAREDE/RESGATE
        while (luz(6) > 14)
        {
            motor(200, 200);

            //viu parede!
            if (bc.distance(0) < 20)
            {
                motor(100, 100);
                delay(500);
                

                if (luz(6) > 68)
                {
                    bc.onTFRot(500, 90);
                    motor(0, 0);
                    val1 = true;//parede
                }
            }
        }      

        //DESPEJAR VITIMA 
        motor(0, 0);
        bc.resetTimer();
        descer(50, 1900);
        
        while (bc.hasVictims())
        {
            delay(10);
            if(bc.timer() > 2500){
                rot(500,45);
                motor(1000,1000);
                delay(200);
                motor(0,0);
                delay(1000);
                rot(500,45);               

                val1 = true;
                goto continuar4;
            }
        }
        continuar4:

        while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
        {
            bc.turnActuatorDown(100);
        }
        while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
        {
            bc.turnActuatorUp(100);
        }

        //VOLTAR A PEGAR BOLINHA
        do
        {
            subir(150, 100);
            print(1, bc.angleActuator().ToString());
        } while (bc.angleActuator() > 290);

        if (val1)
        {   
            rot(500,45);
            motor(-1000,-1000);
            delay(900);
            
            rot(500,-50);

            motor(0,0);
           
            distancia = 190;
            cronometro = 3500;
            val1 = false;
            bc.resetTimer(); 
            goto terminar;
        }
        else if (!val1)
        {
            {//VIRAR

                // NORTE direita 
                if (bc.compass() > 0 && bc.compass() < 45)
                {
                    print(1, "NORTE ATIVADO");
                    rot(1000, 45);
                    do
                    {
                        motor(-1000, 1000);
                    } while (bc.compass() <= 90);
                }

                // NORTE esquerda
                else if (bc.compass() > 315 && bc.compass() < 360)
                {
                    print(1, "NORTE ATIVADO");
                    rot(1000, 45);
                    do
                    {
                        motor(-1000, 1000);
                    } while (bc.compass() <= 90);
                }

                // OESTE
                else if (bc.compass() > 225 && bc.compass() < 315)
                {
                    print(1, "OESTE ATIVADO");
                    do
                    {
                        motor(-1000, 1000);
                    } while (bc.compass() < 359);
                }

                // SUL
                else if (bc.compass() > 135 && bc.compass() < 225)
                {
                    print(1, "SUL ATIVADO");
                    do
                    {
                        motor(-1000, 1000);
                    } while (bc.compass() < 270);
                }

                // LESTE
                else if (bc.compass() > 45 && bc.compass() < 135)
                {
                    print(1, "LESTE ATIVADO");
                    do
                    {
                        motor(-1000, 1000);
                    } while (bc.compass() < 180);
                }
            }

            rot(500,45);
            motor(-1000,-1000);
            delay(900);         
            
            rot(500,-50);

            motor(0,0);
                
            distancia = 190;
            cronometro = 3600;
            bc.resetTimer();     
        }

    }

terminar:   
    print(1, "terminei2");
    bc.resetTimer();
};

Action achouBolinha = () =>{
    {//De Frente pra VITIMA

        motor(-300, -300);
        delay(150);
        {
            if (bc.compass() > 0 && bc.compass() < 45)// NORTE direita 
            {
                print(1, "NORTE ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 0 && bc.compass() < 90);
            }

            else if (bc.compass() > 315 && bc.compass() < 360)// NORTE esquerda
            {
                print(1, "NORTE ATIVADO");
                bc.onTFRot(1000, 45);
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 0 && bc.compass() < 90);
            }

            else if (bc.compass() > 225 && bc.compass() < 315)// OESTE
            {
                print(1, "OESTE ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 225 && bc.compass() < 360);
            }

            else if (bc.compass() > 135 && bc.compass() < 225)// SUL
            {
                print(1, "SUL ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 135 && bc.compass() < 270);
            }

            else if (bc.compass() > 45 && bc.compass() < 135)// LESTE
            {
                print(1, "LESTE ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 45 && bc.compass() < 180);
            }
            motor(0, 0);
        }
    }

    {//Descer o Atuador
        descer(150, 600);
    }

    {//Pegar a VITIMA
        currentTime = bc.millis();
        while (!bc.hasVictims() && (bc.millis() - currentTime) <= 3000)
        {
            motor(300, 300);
        }
    }

    {//encaixar VITIMA
        motor(1000, 1000);
        delay(500);
        subir(90, 600);
    }
   
    //Verificar VITIMA
    if (!bc.hasVictims())
    {
        print(1, "NAO TENHO");
        
        while (!bc.touch(0))
        {
            motor(-1000, -1000);
        }

        bc.onTFRot(500, -90);
        
        while(luz(6) > 13)
        {
        motor(1000, 1000);    
        }
        
        bc.resetTimer();
        goto terminar;

    }
    else if (bc.hasVictims())
    {
        print(1, "TENHO");

        motor(0, 0);
        delay(500);           // 800
        motor(100, 100);     // 70 70
        delay(500);         // 800
        motor(-70, -70);   // 50 50
        delay(800);

        if (!bc.hasVictims())
        {
            print(1, "NAO TENHO");
            while (!bc.touch(0))
            {
                motor(-1000, -1000);
            }

            bc.onTFRot(500, -90);

            while(luz(6) > 13)
            {
                motor(1000, 1000);    
            }
        
            bc.resetTimer();
            goto terminar;
        }

        //CHEGAR NA PAREDE
        while (!bc.touch(0))
        {
            motor(-300, -300);
        }

        //FICAR RETO NO RESGATE
        bc.turnActuatorDown(400);
        bc.onTFRot(500, -90);

        //CHEGAR NO RESGATE
        while (luz(6) > 20)
        {
            motor(200, 200);
        }
        motor(0, 0);
        delay(10);

        //DESPEJAR VITIMA 
        descer(150, 400);
        while (bc.hasVictims())
        {
            delay(10);
        }
        
        //VOLTAR A PEGAR BOLINHA
        while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
        {
            bc.turnActuatorDown(10);
        }
        while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
        {
            bc.turnActuatorUp(10);
        }
        
        do
        {
            subir(150, 100);
            print(1, bc.angleActuator().ToString());
        } while (bc.angleActuator() > 290);
    
        motor(-300, -300);
        delay(700);

        bc.resetTimer();
        goto terminar;
    }
terminar:
    print(1, "terminei1");
    bc.resetTimer();
};

Action resgateContinua = () =>{
    while (true)//PROCURAR MAIS BOLINHA!!!
    {
        bc.resetTimer();
        while (true)
        {
            motor(-200, -200);
               
            if(bc.distance(1) > 45 && bc.distance(1) < distancia){//220
                achouBolinha();
            }
            else if(bc.distance(1) < 42){
                
                motor(-200,-200);
                delay(150);
                
                rot(500,90); 
                motor(0, 0);
                
                motor(400,400);
                delay(500);

                while(!bc.touch(0)){
                    motor(-500,-500);                       
                }

                rot(500,-90);

                while(luz(6) > 14){
                    motor(500,500);
                }

                bc.resetTimer();
            }                 
            
            else if(bc.timer() > cronometro){
                break;
            }
        }
            
        {//VIRAR

            print(3,"virando");
            // NORTE direita  -> OESTE
            if (bc.compass() > 0 && bc.compass() < 45)
            {
                print(1, "NORTE ATIVADO");
                rot(1000, -45);
                do
                {
                    motor(1000, -1000);
                } while (bc.compass() >= 270);
            }

            // NORTE esquerda -> OESTE
            else if (bc.compass() > 315 && bc.compass() < 360)
            {
                print(1, "NORTE ATIVADO");
                rot(1000, -45);
                do
                {
                    motor(1000, -1000);
                } while (bc.compass() >= 270);
            }

            // OESTE -> SUL
            else if (bc.compass() > 225 && bc.compass() < 315)
            {
                print(1, "OESTE ATIVADO");
                do
                {
                    motor(1000, -1000);
                } while (bc.compass() > 180);
            }

            // SUL -> LESTE
            else if (bc.compass() > 135 && bc.compass() < 225)
            {
                print(1, "SUL ATIVADO");
                do
                {
                    motor(1000, -1000);
                } while (bc.compass() > 90);
            }

            // LESTE -> NORTE
            else if (bc.compass() > 45 && bc.compass() < 135)
            {
                print(1, "LESTE ATIVADO");
                do
                {
                    motor(1000, -1000);
                } while (bc.compass() > 1);
            }
        }       
        motor(-300,-300);
        delay(1200);
        
        print(1, "cabei");
        bc.resetTimer();

        while(true)
        {
            motor(-300, -300);

            if (bc.distance(1) > 45 && bc.distance(1) < distancia2)//230
            {
                achouBolinha2();          
            }
            else if(bc.timer() > 2600){
                break;
            }
        }
        bc.turnLedOn(250,100,250);
    }
};

Action resgate = () =>{
    while (true)
    {
        //FRENTE E PROCURAR BOLINHA      
        motor(300, 300);

        //BOLINHA NA FRENTE BRANCA
        if (luz(6) < 59 && luz(6) > 56 && bc.distance(2) < 12)
        {
                {//DAR RÉ
                    motor(-200, -200);
                    delay(700);
                    motor(0, 0);
                }

                {//DESCER ATUADOR
                    descer(150, 600);
                }

                {//PEGAR BOLINHA
                    currentTime = bc.millis();
                    while (!bc.hasVictims() && (bc.millis() - currentTime) <= 1500)
                    {
                        motor(300, 300);
                    }
                }

                {//VERIFICAR E ENTREGAR VITIMA

                    if (!bc.hasVictims())
                    {
                        print(1, "NAO TENHO");
                        do
                        {
                            subir(150, 100);
                            print(1, bc.angleActuator().ToString());
                        } while (bc.angleActuator() > 290);

                        while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
                        {
                            bc.turnActuatorUp(100);
                        }
                        while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
                        {
                            bc.turnActuatorDown(100);
                        }                      
                    }

                    else if (bc.hasVictims())
                    {
                        print(1, "TENHO");

                        motor(300, 300);
                        delay(300);
                        motor(100, 100);
                        subir(90, 800);
                        bc.turnActuatorDown(800);
                        motor(0, 0);
                        delay(800);

                        if (!bc.hasVictims())
                        {
                            print(1, "NAO TENHO");

                            while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
                            {
                                bc.turnActuatorUp(100);
                            }
                            while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
                            {
                                bc.turnActuatorDown(100);
                            }
                        }

                        while (bc.hasVictims())
                        {
                            motor(200, 200);

                            //RESGATE
                            if (bc.distance(0) < 110 && bc.distance(0) > 45 && luz(6) < 13)
                            {
                                print(1, "ACHEI RESGATE!!");
                                motor(0, 0);

                                while (bc.hasVictims())
                                {
                                    descer(150, 10);
                                    delay(10);
                                }

                                while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
                                {
                                    bc.turnActuatorDown(100);
                                }
                                while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
                                {
                                    bc.turnActuatorUp(100);
                                }

                                //SUBIR GARRA
                                do
                                {
                                    subir(150, 100);
                                    print(1, bc.angleActuator().ToString());
                                } while (bc.angleActuator() > 290);

                                bc.resetTimer();
                                resgateContinua();
                            }

                            //PAREDE
                            else if (bc.distance(0) < 15 && bc.distance(2) < 15)
                            {
                                // NORTE direita 
                                if (bc.compass() > 0 && bc.compass() < 45)
                                {
                                    print(1, "NORTE ATIVADO");
                                    do
                                    {
                                        motor(-1000, 1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() > 0 && bc.compass() < 90);
                                }

                                // NORTE esquerda
                                else if (bc.compass() > 315 && bc.compass() < 360)
                                {
                                    print(1, "NORTE ATIVADO");
                                    bc.onTFRot(1000, 45);
                                    do
                                    {
                                        motor(-1000, 1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() > 0 && bc.compass() < 90);
                                }

                                // OESTE
                                else if (bc.compass() > 225 && bc.compass() < 315)
                                {
                                    print(1, "OESTE ATIVADO");
                                    do
                                    {
                                        motor(-1000, 1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() > 225 && bc.compass() < 360);
                                }

                                // SUL
                                else if (bc.compass() > 135 && bc.compass() < 225)
                                {
                                    print(1, "SUL ATIVADO");
                                    do
                                    {
                                        motor(-1000, 1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() > 135 && bc.compass() < 270);
                                }

                                // LESTE
                                else if (bc.compass() > 45 && bc.compass() < 135)
                                {
                                    print(1, "LESTE ATIVADO");
                                    do
                                    {
                                        motor(-1000, 1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() > 45 && bc.compass() < 180);
                                }
                            }
                        }
                    }
                }
        }

        //BOLINHA NA FRENTE PRETA
        if (luz(6) < 17 && luz(6) > 14 && bc.distance(2) < 12)
        {
                {//DAR RÉ
                    motor(-200, -200);
                    delay(500);
                    motor(0, 0);
                }

                {//DESCER ATUADOR
                    descer(150, 500);
                }

                {//PEGAR BOLINHA
                    currentTime = bc.millis();
                    while (!bc.hasVictims() && (bc.millis() - currentTime) <= 1500)
                    {
                        motor(200, 200);
                    }
                }

                {//VERIFICAR E ENTREGAR VITIMA

                    if (!bc.hasVictims())
                    {
                        print(1, "NAO TENHO");
                        do
                        {
                            subir(150, 100);
                            print(1, bc.angleActuator().ToString());
                        } while (bc.angleActuator() > 290);

                        while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
                        {
                            bc.turnActuatorUp(100);
                        }
                        while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
                        {
                            bc.turnActuatorDown(100);
                        }     
                    }

                    else if (bc.hasVictims())
                    {
                        print(1, "TENHO");

                        motor(300, 300);
                        delay(300);
                        motor(100, 100);
                        subir(90, 800);
                        bc.turnActuatorUp(800);
                        motor(0, 0);
                        delay(800);

                        if (!bc.hasVictims())
                        {
                            print(1, "NAO TENHO");

                            while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
                            {
                                bc.turnActuatorUp(100);
                            }
                            while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
                            {
                                bc.turnActuatorDown(100);
                            }
                        }

                        while (bc.hasVictims())
                        {
                            motor(200, 200);

                            //RESGATE
                            if (bc.distance(0) < 110 && bc.distance(0) > 45 && luz(6) < 13)
                            {
                                print(1, "ACHEI RESGATE!!");
                                motor(0, 0);

                                while (bc.hasVictims())
                                {
                                    descer(150, 10);
                                    delay(10);
                                }

                                while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
                                {
                                    bc.turnActuatorDown(100);
                                }
                                while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
                                {
                                    bc.turnActuatorUp(100);
                                }

                                //SUBIR GARRA
                                do
                                {
                                    subir(150, 100);
                                    print(1, bc.angleActuator().ToString());
                                } while (bc.angleActuator() > 290);

                                bc.resetTimer();
                                resgateContinua();
                            }

                            //PAREDE
                            else if (bc.distance(0) < 15 && bc.distance(2) < 15)
                            {
                                // NORTE direita 
                                if (bc.compass() > 0 && bc.compass() < 45)
                                {
                                    print(1, "NORTE ATIVADO");
                                    do
                                    {
                                        motor(-1000, 1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() > 0 && bc.compass() < 90);
                                }

                                // NORTE esquerda
                                else if (bc.compass() > 315 && bc.compass() < 360)
                                {
                                    print(1, "NORTE ATIVADO");
                                    bc.onTFRot(1000, 45);
                                    do
                                    {
                                        motor(-1000, 1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() > 0 && bc.compass() < 90);
                                }

                                // OESTE
                                else if (bc.compass() > 225 && bc.compass() < 315)
                                {
                                    print(1, "OESTE ATIVADO");
                                    do
                                    {
                                        motor(-1000, 1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() > 225 && bc.compass() < 360);
                                }

                                // SUL
                                else if (bc.compass() > 135 && bc.compass() < 225)
                                {
                                    print(1, "SUL ATIVADO");
                                    do
                                    {
                                        motor(-1000, 1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() > 135 && bc.compass() < 270);
                                }

                                // LESTE
                                else if (bc.compass() > 45 && bc.compass() < 135)
                                {
                                    print(1, "LESTE ATIVADO");
                                    do
                                    {
                                        motor(-1000, 1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() > 45 && bc.compass() < 180);
                                }
                            }
                        }
                    }
                }
        }

        //BOLINHA NA DIREITA
        if (bc.distance(1) > 40 && bc.distance(1) < 147)
        {
                print(1, "bolinha direita 1°vez");

                {//FRENTE COM A BOLINHA
                    motor(300, 300);
                    delay(80);

                    // NORTE direita 
                    if (bc.compass() > 0 && bc.compass() < 45)
                    {
                        print(1, "NORTE ATIVADO");
                        do
                        {
                            motor(-1000, 1000);
                        } while (bc.compass() > 0 && bc.compass() < 90);
                    }

                    // NORTE esquerda
                    else if (bc.compass() > 315 && bc.compass() < 360)
                    {
                        print(1, "NORTE ATIVADO");
                        bc.onTFRot(1000, 45);
                        do
                        {
                            motor(-1000, 1000);
                        } while (bc.compass() > 0 && bc.compass() < 90);
                    }

                    // OESTE
                    else if (bc.compass() > 225 && bc.compass() < 315)
                    {
                        print(1, "OESTE ATIVADO");
                        do
                        {
                            motor(-1000, 1000);
                        } while (bc.compass() > 225 && bc.compass() < 360);
                    }

                    // SUL
                    else if (bc.compass() > 135 && bc.compass() < 225)
                    {
                        print(1, "SUL ATIVADO");
                        do
                        {
                            motor(-1000, 1000);
                        } while (bc.compass() > 135 && bc.compass() < 270);
                    }

                    // LESTE
                    else if (bc.compass() > 45 && bc.compass() < 135)
                    {
                        print(1, "LESTE ATIVADO");
                        do
                        {
                            motor(-1000, 1000);
                        } while (bc.compass() > 45 && bc.compass() < 180);
                    }

                    motor(0, 0);
                }

                //classificar bolinha
                    //bolinha preta
                        //volta 
                            //procura bolinha dnv

                    //bolinha branca
                    
                {//DESCER O ATUADOR
                    descer(150, 600);
                }

                {//PEGAR BOLINHA
                    currentTime = bc.millis();
                    while (!bc.hasVictims() && (bc.millis() - currentTime) <= 3000)
                    {
                        motor(300, 300);
                    }
                }

                {//VERIFICAR E ENTREGAR VITIMA
                    if (!bc.hasVictims())
                    {
                        print(1, "NAO TENHO");
                        do
                        {
                            subir(150, 100);
                            print(1, bc.angleActuator().ToString());
                        } while (bc.angleActuator() > 290);

                        while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
                        {
                            bc.turnActuatorDown(100);
                        }
                        while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
                        {
                            bc.turnActuatorUp(100);
                        }

                        while (!bc.touch(0))
                        {
                            motor(-300, -300);
                        }

                        bc.onTFRot(500, -90);
                        motor(0, 0);
                    }
                    else if (bc.hasVictims())
                    {
                        print(1, "TENHO");

                        motor(300, 300);
                        delay(500);
                        motor(100, 100);
                        subir(90, 800);                     
                        motor(0, 0);
                        delay(800);

                        if (!bc.hasVictims())
                        {
                            print(1, "NAO TENHO1");

                            while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
                            {
                                bc.turnActuatorDown(100);
                            }
                            while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
                            {
                                bc.turnActuatorUp(100);
                            }

                            while (!bc.touch(0))
                            {
                                motor(-1000, -1000);
                            }

                            bc.onTFRot(500, -90);
                            motor(100, 100);
                            delay(100);
                            goto terminar;
                        }

                        //RESGATE onde?
                        if (resgateVal)    //RESGATE DIREITA
                        {
                            while (bc.hasVictims())
                            {
                                //VERIFICAR RESGATE
                                if (luz(3) < 13)
                                {
                                    print(1, "ACHEI RESGATE!!");
                                    motor(0, 0);

                                    while (bc.hasVictims())
                                    {
                                        descer(150, 10);
                                        delay(10);
                                    }

                                    while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
                                    {
                                        bc.turnActuatorDown(100);
                                    }
                                    while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
                                    {
                                        bc.turnActuatorUp(100);
                                    }

                                    //SUBIR GARRA
                                    do
                                    {
                                        subir(150, 100);
                                        print(1, bc.angleActuator().ToString());
                                    } while (bc.angleActuator() > 290);

                                    //trabalhar com o iGOR !!!!!!!!!!!!!!!!!!!!!!
                                    bc.onTFRot(500, 90);

                                    bc.resetTimer();
                                    resgateContinua();
                                }

                                //VIRAR NA PAREDE
                                else if (bc.distance(0) < 20)
                                {
                                    // NORTE direita 
                                    if (bc.compass() > 0 && bc.compass() < 45)
                                    {
                                        print(1, "NORTE ATIVADO");
                                        do
                                        {
                                            motor(-1000, 1000);
                                            print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                        } while (bc.compass() > 0 && bc.compass() < 90);
                                    }

                                    // NORTE esquerda
                                    else if (bc.compass() > 315 && bc.compass() < 360)
                                    {
                                        print(1, "NORTE ATIVADO");
                                        bc.onTFRot(1000, 45);
                                        do
                                        {
                                            motor(-1000, 1000);
                                            print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                        } while (bc.compass() > 0 && bc.compass() < 90);
                                    }

                                    // OESTE
                                    else if (bc.compass() > 225 && bc.compass() < 315)
                                    {
                                        print(1, "OESTE ATIVADO");
                                        do
                                        {
                                            motor(-1000, 1000);
                                            print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                        } while (bc.compass() > 225 && bc.compass() < 360);
                                    }

                                    // SUL
                                    else if (bc.compass() > 135 && bc.compass() < 225)
                                    {
                                        print(1, "SUL ATIVADO");
                                        do
                                        {
                                            motor(-1000, 1000);
                                            print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                        } while (bc.compass() > 135 && bc.compass() < 270);
                                    }

                                    // LESTE
                                    else if (bc.compass() > 45 && bc.compass() < 135)
                                    {
                                        print(1, "LESTE ATIVADO");
                                        do
                                        {
                                            motor(-1000, 1000);
                                            print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                        } while (bc.compass() > 45 && bc.compass() < 180);
                                    }
                                }

                                //DESVIAR BOLINHA PRETA
                                if (luz(3) > 25 && luz(3) < 35)
                                {

                                }

                                //DESVIAR BOLINHA BRANCA
                                if (luz(3) > 25 && luz(3) < 35)
                                {

                                }

                                //FRENTE
                                else { motor(200, 200); }
                            }
                        }
                        else              //RESGATE EM OUTROS LUGARES
                        {
                            //CHEGAR NA PAREDE
                            while (!bc.touch(0))
                            {
                                motor(-200, -200);
                            }
                            bc.turnActuatorDown(800);

                            //FICAR RETO PRA PAREDE                                         
                            bc.onTFRot(500, -30);
                            {//virar....
                                // NORTE
                                if (bc.compass() > 0 && bc.compass() < 44)
                                {
                                    print(1, "NORTE ATIVADO");
                                    do
                                    {
                                        motor(1000, -1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() >= 270);
                                }
                                
                                // NORTE
                                if (bc.compass() > 319 && bc.compass() < 360)
                                {
                                    print(1, "NORTE ATIVADO");
                                    do
                                    {
                                        motor(1000, -1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() >= 270);
                                }

                                // OESTE
                                else if (bc.compass() > 228 && bc.compass() < 311)
                                {
                                    print(1, "OESTE ATIVADO");
                                    do
                                    {
                                        motor(1000, -1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() >= 180);
                                }

                                // SUL
                                else if (bc.compass() > 136 && bc.compass() < 223)
                                {
                                    print(1, "SUL ATIVADO");
                                    do
                                    {
                                        motor(1000, -1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() >= 90);
                                }

                                // LESTE
                                else if (bc.compass() > 45 && bc.compass() < 134)
                                {
                                    print(1, "LESTE ATIVADO");
                                    do
                                    {
                                        motor(1000, -1000);
                                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                    } while (bc.compass() >= 0);
                                }
                            }    
                            
                            while (bc.hasVictims())
                            {
                                //VERIFICAR RESGATE
                                if (luz(6) < 13)
                                {
                                    print(1, "ACHEI RESGATE!!");
                                    motor(0, 0);

                                    while (bc.hasVictims())
                                    {
                                        descer(150, 10);
                                        delay(10);
                                    }

                                    while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
                                    {
                                        bc.turnActuatorDown(100);
                                    }
                                    while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
                                    {
                                        bc.turnActuatorUp(100);
                                    }

                                    //SUBIR GARRA
                                    do
                                    {
                                        subir(150, 100);
                                        print(1, bc.angleActuator().ToString());
                                    } while (bc.angleActuator() > 290);

                                    motor(-300,-300);
                                    delay(500);

                                    bc.resetTimer();
                                    resgateContinua();
                                }

                                //VIRAR NA PAREDE
                                else if (bc.distance(0) < 25)
                                {
                                    bc.turnActuatorUp(800);
                                    
                                    // NORTE direita 
                                    if (bc.compass() > 0 && bc.compass() < 45)
                                    {
                                        print(1, "NORTE ATIVADO");
                                        do
                                        {
                                            motor(-1000, 1000);
                                            print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                        } while (bc.compass() > 0 && bc.compass() < 90);
                                    }

                                    // NORTE esquerda
                                    else if (bc.compass() > 315 && bc.compass() < 360)
                                    {
                                        print(1, "NORTE ATIVADO");
                                        bc.onTFRot(1000, 45);
                                        do
                                        {
                                            motor(-1000, 1000);
                                            print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                        } while (bc.compass() > 0 && bc.compass() < 90);
                                    }

                                    // OESTE
                                    else if (bc.compass() > 225 && bc.compass() < 315)
                                    {
                                        print(1, "OESTE ATIVADO");
                                        do
                                        {
                                            motor(-1000, 1000);
                                            print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                        } while (bc.compass() > 225 && bc.compass() < 360);
                                    }

                                    // SUL
                                    else if (bc.compass() > 135 && bc.compass() < 225)
                                    {
                                        print(1, "SUL ATIVADO");
                                        do
                                        {
                                            motor(-1000, 1000);
                                            print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                        } while (bc.compass() > 135 && bc.compass() < 270);
                                    }

                                    // LESTE
                                    else if (bc.compass() > 45 && bc.compass() < 135)
                                    {
                                        print(1, "LESTE ATIVADO");
                                        do
                                        {
                                            motor(-1000, 1000);
                                            print(2, "BUSSOLA" + " " + bc.compass().ToString());
                                        } while (bc.compass() > 45 && bc.compass() < 180);
                                    }

                                    motor(0,0);
                                    bc.turnActuatorDown(800);
                                }

                                //DESVIAR BOLINHA PRETA
                                if (luz(3) > 25 && luz(3) < 35)
                                {
                                    
                                }

                                //DESVIAR BOLINHA BRANCA
                                if (luz(3) > 25 && luz(3) < 35)
                                {

                                }

                                //FRENTE
                                else { motor(300, 300); }
                            }
                        }
                    }
                }
        }

        //AREA DE RESGATE
        if (luz(6) < 23)
        {
                motor(-1000, -1000);
                delay(200);
                rot(500, 45);
                motor(1000, 1000);
                delay(2400);

                // NORTE direita 
                if (bc.compass() > 0 && bc.compass() < 65)
                {
                    print(1, "NORTE ATIVADO");
                    do
                    {
                        motor(-1000, 1000);
                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                    } while (bc.compass() > 0 && bc.compass() < 90);
                }

                // NORTE esquerda
                else if (bc.compass() > 325 && bc.compass() < 360)
                {
                    print(1, "NORTE ATIVADO");
                    bc.onTFRot(1000, 45);
                    do
                    {
                        motor(-1000, 1000);
                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                    } while (bc.compass() > 0 && bc.compass() < 90);
                }

                // OESTE
                else if (bc.compass() > 225 && bc.compass() < 320)
                {
                    print(1, "OESTE ATIVADO");
                    do
                    {
                        motor(-1000, 1000);
                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                    } while (bc.compass() > 225 && bc.compass() < 360);
                }

                // SUL
                else if (bc.compass() > 170 && bc.compass() < 225)
                {
                    print(1, "SUL ATIVADO");
                    do
                    {
                        motor(-1000, 1000);
                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                    } while (bc.compass() > 135 && bc.compass() < 270);
                }

                // LESTE
                else if (bc.compass() > 70 && bc.compass() < 165)
                {
                    print(1, "LESTE ATIVADO");
                    do
                    {
                        motor(-1000, 1000);
                        print(2, "BUSSOLA" + " " + bc.compass().ToString());
                    } while (bc.compass() > 45 && bc.compass() < 180);
                }

                motor(0, 0);
        }       

        //PAREDE
        else if (bc.distance(0) < 15 && bc.distance(2) < 15)
        {
            // NORTE direita 
            if (bc.compass() > 0 && bc.compass() < 45)
            {
                print(1, "NORTE ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 0 && bc.compass() < 90);
            }

            // NORTE esquerda
            else if (bc.compass() > 315 && bc.compass() < 360)
            {
                print(1, "NORTE ATIVADO");
                bc.onTFRot(1000, 45);
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 0 && bc.compass() < 90);
            }

            // OESTE
            else if (bc.compass() > 225 && bc.compass() < 315)
            {
                print(1, "OESTE ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 225 && bc.compass() < 360);
            }

            // SUL
            else if (bc.compass() > 135 && bc.compass() < 225)
            {
                print(1, "SUL ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 135 && bc.compass() < 270);
            }

            // LESTE
            else if (bc.compass() > 45 && bc.compass() < 135)
            {
                print(1, "LESTE ATIVADO");
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() > 45 && bc.compass() < 180);
            }
        }

        terminar:
        print(1,"terminei");   
    }
};

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
int tempoDeIda = 470;
int tempoDeVolta = 300;
int Angulo = 35;
int angulo2 = 2;
int tempoCirculo = 1700;

//ESQUERDO
Action CondiQuadradoEsquerdo = () =>{
    while(true){
        
        motor(100,100);

        if(cor(0)=="PRETO"){//esquerdo
            bc.turnLedOn(200,0,200);

            motor(200, 200);
            delay(650);

            rot(500,85);//esquerdo

            motor(-200,-200);
            delay(300);
            motor(0,0);

            break;
        }

        if(bc.timer() > 1100){
            break;
        }
    }
};

Action CondiCirculoEsquerdo = () =>{
    while(true){
        
        motor(120,120);

        if(cor(0)=="PRETO"){//esquerdo
            bc.turnLedOn(200,0,200);

            rot(500,-20);//wip

            motor(200, 200);
            delay(650);

            rot(500,90);//esquerdo

            motor(-200,-200);
            delay(400);

            break;
        }

        if(bc.timer() > tempoCirculo){
            break;
        }
    }   
};
//////////

//DIREITO
Action CondiQuadradoDireito= () =>{
    while(true){
        
        motor(100,100);

        if(cor(4)=="PRETO"){//esquerdo
            bc.turnLedOn(200,0,200);

            motor(200, 200);
            delay(650);

            rot(500,-85);//esquerdo

            motor(-200,-200);
            delay(300);
            motor(0,0);

            break;
        }

        if(bc.timer() > 1100){
            break;
        }
    }
};

Action CondiCirculoDireito = () =>{
    while(true){
        
        motor(120,120);

        if(cor(4)=="PRETO"){//esquerdo
            bc.turnLedOn(200,0,200);

            rot(500,20);//wip

            motor(200, 200);
            delay(650);

            rot(500,-90);//esquerdo

            motor(-200,-200);
            delay(400);

            break;
        }

        if(bc.timer() > tempoCirculo){
            break;
        }
    }  
};
//////////

Action verde = () =>{
    //DOIS VERDES
    if (cor(1) == "VERDE" && cor(3) == "VERDE")
    {

        bc.resetTimer();

        while (cor(1) != "PRETO" && cor(3) != "PRETO")
        {
            motor(50, 50);

            if (bc.timer() >= 1000)
            {
                val2 = true;
                break;
            }
        }
        motor(0, 0);
        bc.resetTimer();

        if (val2)
        {
            motor(300, 300);
            delay(300);
            goto continuar;
        }

        else
        {
            // NORTE DIREITA
            if (bc.compass() >= 0 && bc.compass() < 45)
            {
                print(1, "NORTE ATIVADO");
                bc.onTFRot(1000, 45);
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() < 180);
                goto continuar;
            }

            // NORTE ESQUERDA
            else if (bc.compass() > 315 && bc.compass() <= 360)
            {
                print(1, "NORTE ATIVADO");
                bc.onTFRot(1000, 45);
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() < 180);
                goto continuar;
            }

            // OESTE
            else if (bc.compass() > 225 && bc.compass() < 315)
            {
                print(1, "OESTE ATIVADO");
                bc.onTFRot(1000, 120);
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() <= 90);
                goto continuar;
            }

            // SUL
            else if (bc.compass() > 135 && bc.compass() < 225)
            {
                print(1, "SUL ATIVADO");
                bc.onTFRot(1000, 45);
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() <= 359);
                goto continuar;
            }

            // LESTE
            else if (bc.compass() > 45 && bc.compass() < 135)
            {
                print(1, "LESTE ATIVADO");
                bc.onTFRot(1000, 45);
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() < 270);
            }
        }

    continuar:
        motor(0, 0);
        val2 = false;
    }

    //ESQUERDA
    if (cor(4) == "VERDE" || cor(3) == "VERDE" || cor(4) == "AMARELO" || cor(3) == "AMARELO")
    {
        bc.turnLedOn(0, 250, 0);
        print(1, "VERDE = ESQUERDA");
        
        bc.resetTimer();

        while(cor(4) != "PRETO" && cor(3) != "PRETO" && cor(1) != "PRETO" && cor(0) != "PRETO"){        
            motor(50,50);

            if (bc.timer() >= 1300)
            {
                val2 = true;
                break;
            }      
        }

        if (val2)
        {
            motor(300, 300);
            delay(300);             
        }
        else if(!val2){
            //FRENTE
            motor(1000, 1000);
            delay(tempoDeIda);

            //VIRAR (ate achar preto)
            rot(500, -(Angulo));
            while (cor(2) != "PRETO")
            {
                motor(1000, -1000);
            }
            rot(500, -angulo2);
            
            {//VERIFICAÇÂO DE quadrado ESQUERDO
                if((bc.compass() <= 359 && bc.compass() >= 345) || (bc.compass() <= 20 && bc.compass() > 0)){
                    print(3,"QUADRADO NORTE");
                    quadrado = true;
                }

                else if(bc.compass() <= 280 && bc.compass() >= 250){
                    print(3,"QUADRADO OESTE");
                    quadrado = true;
                }

                else if(bc.compass() <= 185 && bc.compass() >= 170){
                    print(3,"QUADRADO SUL");
                    quadrado = true;
                }

                else if(bc.compass() <= 100 && bc.compass() >= 80){
                    print(3,"QUADRADO LESTE");
                    quadrado = true;
                }  
            }
            
            //VOLTA
            motor(-200, -200);
            delay(tempoDeVolta);

            bc.resetTimer();

            if(quadrado){
                CondiQuadradoEsquerdo();
            }
            else{
                CondiCirculoEsquerdo();
            }
        } 
        val2 = false;
        quadrado = false;
        bc.turnLedOff();
        bc.resetTimer();
    }

    //DIREITA
    if (cor(1) == "VERDE" || cor(0) == "VERDE" || cor(1) == "AMARELO" || cor(0) == "AMARELO")
    {
        bc.turnLedOn(0, 250, 0);
        print(1, "VERDE = DIREITA");
        
        bc.resetTimer();

        while(cor(4) != "PRETO" && cor(3) != "PRETO" && cor(1) != "PRETO" && cor(0) != "PRETO"){
            motor(50,50);

            if(bc.timer() >= 1300){
                val2 = true;
                break;
            }
        }

        if(val2){
            motor(300,300);
            delay(300);
        }
        else if(!val2){
            //FRENTE
            motor(1000, 1000);
            delay(tempoDeIda);

            //VIRAR (ate achar preto)
            rot(500, Angulo);
            while (cor(2) != "PRETO")
            {
                motor(-1000, 1000);
            }

            rot(500, angulo2);

            {//VERIFICAÇÂO DE quadrado DIREITO
                if((bc.compass() < 360 && bc.compass() >= 345) || (bc.compass() <= 20 && bc.compass() > 0)){
                    print(3,"QUADRADO NORTE");
                    quadrado = true;
                }

                else if(bc.compass() <= 280 && bc.compass() >= 250){
                    print(3,"QUADRADO OESTE");
                    quadrado = true;
                }

                else if(bc.compass() <= 185 && bc.compass() >= 170){
                    print(3,"QUADRADO SUL");
                    quadrado = true;
                }

                else if(bc.compass() <= 100 && bc.compass() >= 80){
                    print(3,"QUADRADO LESTE");
                    quadrado = true;
                }  
            }

            //VOLTA
            motor(-200, -200);
            delay(tempoDeVolta);

            bc.resetTimer();

            if(quadrado){
                CondiQuadradoDireito();
            }
            else{
                CondiCirculoDireito();
            }
        }  
        val2 = false;
        quadrado = false;
        bc.turnLedOff();
        bc.resetTimer();
    }
};

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Action Obstaculo = () =>{
    if (bc.distance(2) < 10)
    {
        bc.turnLedOn(255,255,0);
        print(3, "--=OBSTÁCULO=--");
        bc.turnLedOn(255,255,0);

        {//virar
            // NORTE 
            if ((bc.compass() >= 0 && bc.compass() < 45) || (bc.compass() > 315 && bc.compass() <= 360))
            {
                print(1, "NORTE ATIVADO");
                bc.onTFRot(1000, 15);
                do
                {
                    motor(-1000, 1000);
                } while (bc.compass() < 45);
                goto continuar;
            }
            // OESTE
            else if (bc.compass() > 225 && bc.compass() < 315)
            {
                print(1, "OESTE ATIVADO");
                bc.onTFRot(1000, 45);
                do
                {
                    motor(-1000, 1000);
                } while (bc.compass() < 315);
                goto continuar;
            }
            // SUL
            else if (bc.compass() > 135 && bc.compass() < 225)
            {
                print(1, "SUL ATIVADO");
                bc.onTFRot(1000, 45);
                do
                {
                    motor(-1000, 1000);
                } while (bc.compass() < 225);
                goto continuar;
            }
            // LESTE
            else if (bc.compass() > 45 && bc.compass() < 135)
            {
                print(1, "LESTE ATIVADO");
                bc.onTFRot(1000, 45);
                do
                {
                    motor(-1000, 1000);
                    print(2, "BUSSOLA" + " " + bc.compass().ToString());
                } while (bc.compass() < 135);
                goto continuar;
            }
        }

    continuar:   

        bc.resetTimer();
        while (true)
        {
            motor(250, 250); //150 150

            if (luz(3) < black || luz(1) < black)
            {
                motor(1000, 1000);
                delay(450);
                rot(500, 40);
                print(3, " ");
                motor(0, 0);
                break;
            }

            if (bc.timer() >= 800)
            {
                motor(-1000, -1000);
                delay(900);
                rot(500, -80);
                bc.resetTimer();

                while (true)
                {
                    motor(300, 300);//150 150

                    if (luz(3) < black || luz(5) < black)
                    {
                        motor(1000, 1000);
                        delay(450);
                        rot(500, -40);
                        print(3, " ");
                        motor(0, 0);
                        break;
                    }                   
                    
                    if (bc.timer() >= 1000)
                    {   
                        rot(500, 35);
                        motor(250,250);
                        delay(1000);
                        
                        rot(500, 85);
                        
                        while(luz(3) >= black){
                            motor(100,100);
                        }

                        motor(1000, 1000);
                        delay(450);

                        rot(500,-90);
                                      
                        while(bc.touch(0) == false){
                            motor(-200,-200);
                        }

                        print(3, " ");
                        motor(0,0);
                        break;
                    }
                }
                
                print(3, " ");
                break;
            }
        }
    }
};

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
int voltaExtra = 200;
Action condi = () =>{  
    
    if(cor(2) == "VERDE"){
        motor(-200,-200);
        delay(200);
        motor(0,0);
        rot(500,2);
        motor(0,0);
    }

    if(luz(4) < black && luz(3) < black && luz(2) < black ){
        while(luz(1) >= black && luz(5) >= black){
            motor(100,100);
        }
    }
    
    
    //CINZA
    if(((luz(2) > 52 && luz(3) > 52 && luz(4) > 52 && luz(5) > 52) && (luz(2) < 60 && luz(3) < 60 && luz(4) < 60 && luz(5) < 60)) || ((luz(2) > 74 && luz(3) > 74 && luz(4) > 74 && luz(5) > 74) && (luz(2) < 80 && luz(3) < 80 && luz(4) < 80 && luz(5) < 80))){
         
        {//NORTE DIREITA
            if(bc.compass() > 1 && bc.compass() < 45){
                while(bc.compass() > 1 ){
                    motor(1000,-1000);
                }
            }
            //NORTE ESQUERDA
            else if(bc.compass() > 315 && bc.compass() < 359){
                while(bc.compass() < 359){
                    motor(-1000,1000);
                }
            }
            //LESTE DIREITA
            else if(bc.compass() > 90 && bc.compass() < 135){
                while(bc.compass() > 90){
                    motor(1000,-1000);
                }
            }
            //LESTE ESQUERDA
            else if(bc.compass() > 45 && bc.compass() < 90){
                while(bc.compass() < 90){
                    motor(-1000,1000);
                }
            }
            //SUL DIREITA
            else if(bc.compass() > 180 && bc.compass() < 225){
                while(bc.compass() > 180){
                    motor(1000,-1000);
                }
            }
            //SUL ESQUERDA
            else if(bc.compass() > 135 && bc.compass() < 180){
                while(bc.compass() < 180){
                    motor(-1000,1000);
                }
            }
            //OESTE DIREITA
            else if(bc.compass() > 270 && bc.compass() < 315){
                while(bc.compass() > 270){
                    motor(1000,-1000);
                }
            }
            //OESTE ESQUERDA
            else if(bc.compass() > 225 && bc.compass() < 270){
                while(bc.compass() < 270){
                    motor(-1000,1000);
                }
            }
        }

        bc.turnLedOn(250,0,250);
        motor(0,0);

        while (bc.angleActuator() > 290){
          subir(150, 50);
        } 

        while (bc.angleBucket() > 2 && bc.angleBucket() < 52)
        {
            bc.turnActuatorDown(10);
        }
        while (bc.angleBucket() < 358 && bc.angleBucket() > 310)
        {
            bc.turnActuatorUp(10);
        }
             
        while(bc.distance(1) > 300){
            motor(200,200);
        } 
        motor(200,200);
        delay(800);
        bc.turnLedOn(0,250,250);
      
        resgate();
    }
    
    //ESQUERDA
    if ((luz(5) < black|| luz(4) < black) && (luz(1) > black) ){
        bc.turnLedOn("VERMELHO");
        
        //ir pra frente
        while(cor(2) != "BRANCO"){
            motor(200,200);
            delay(50);
        }

        bc.resetTimer();
        
        //ir pra linha
        while(cor(2) != "PRETO"){
            motor(1000,-1000);

            if(bc.timer() >= 2000){
                
                bc.resetTimer();

                while(cor(0) == "BRANCO" && cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO"){
                    motor(-1000,1000);

                    if(bc.timer() >= 2500){
                        rot(500,-20);
                        
                        while(cor(0) == "BRANCO" && cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO"){
                            motor(-100,-100);
                        }

                        motor(-200,-200);
                        delay(500);

                        rot(500,-5);
                        break;
                    }
                }
                motor(-200,-200);
                delay(200);
                break;
            }
        }
        
        //rot(500,-3);
        bc.resetTimer();
        bc.turnLedOff();
    }

    //DIREITA
    if ((luz(1) < black || luz(2) < black) && (luz(5) > black)){
        bc.turnLedOn("AZUL");
        
        //ir pra frente
        while(cor(2) != "BRANCO"){
            motor(200,200);
            delay(50);
        }
        
        bc.resetTimer();

        //ir pra linha
        while(cor(2) != "PRETO"){
            motor(-1000,1000);

            if(bc.timer() >= 2000){
                
                bc.resetTimer();

                while(cor(0) == "BRANCO" && cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO"){
                    motor(1000,-1000);

                    if(bc.timer() >= 2500){
                        rot(500,20);
                        
                        while(cor(0) == "BRANCO" && cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO"){
                            motor(-100,-100);
                        }

                        motor(-200,-200);
                        delay(500);

                        rot(500,5);
                        break;
                    }
                }

                break;
            }
        }
        
        //rot(500, 3);
        bc.resetTimer();
        bc.turnLedOff();
    }

    //BRANCO
    if (cor(4) == "BRANCO" && cor(3) == "BRANCO" && cor(2) == "BRANCO" && cor(1) == "BRANCO" && cor(0) == "BRANCO"){

        if (bc.timer()>= 1500){
            bc.turnLedOn(200, 200, 200);
            currentTime = bc.millis();

            while (cor(4) == "BRANCO" && cor(3) == "BRANCO" && cor(2) == "BRANCO" && cor(1) == "BRANCO" && cor(0) == "BRANCO")
            {
                motor(1000, -1000);
                if (bc.millis() - currentTime > 1500)
                {
                    currentTime = bc.millis();
                    while (cor(4) == "BRANCO" && cor(3) == "BRANCO" && cor(2) == "BRANCO" && cor(1) == "BRANCO" && cor(0) == "BRANCO")
                    {
                        motor(-1000, 1000);
                        if (bc.millis() - currentTime > 3000)
                        {
                            currentTime = 0;
                            rot(1000, -40);

                            while (cor(4) != "PRETO" && cor(3) != "PRETO" && cor(2) != "PRETO" && cor(1) != "PRETO" && cor(0) != "PRETO")
                            {
                                motor(-150, -150);
                            }

                            motor(-200, -200);
                            delay(voltaExtra);
                            bc.turnLedOff();
                            //rot(500,5);
                            break;                           
                        }
                    }
                }
            }
        }
    }
    else { bc.resetTimer(); delay(5);}

    motor(velocidade, velocidade);

    verde();
};

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Action debug = () =>{
    //print(1, cor(4) + "   " + cor(3) + "   " + cor(2) + "   " + cor(1) + "   " + cor(0));
    //print(2, bc.distance(0).ToString() + "   " + bc.distance(1).ToString() + "   " + bc.distance(2).ToString());
    //print(3, bc.compass().ToString() + "   " + tempo2);
    //print(1, bc.angleBucket() + "   " + bc.angleActuator());
    //print(2, luz(1).ToString() + "   " + luz(2).ToString() + "   " + luz(3).ToString() + "   " + luz(4).ToString() + "   " + luz(5).ToString() );

    
};

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//rampa em cima

Func <bool> setup = (resgateVal) =>{
   
    if (bc.distance(1) < 23 || (luz(2) < 60 && luz(2) > 53) && (luz(3) < 60 && luz(3) > 53) && (luz(4) < 60 && luz(4) > 53)){
        bc.turnLedOn(250, 0, 250);
        print(1, "TO PRESO");
        motor(300, 300);
        delay(500);
        print(1, "TO LIVRE");

        do
        {
            subir(150, 100);
            print(1, bc.angleActuator().ToString());
        } while (bc.angleActuator() > 290);

        if (bc.distance(1) < 180)
        {
            resgateVal = true;
        }
        print(3, "Resgate 3°" + " = " + resgateVal.ToString());

        bc.turnLedOn(0, 250, 250);
        resgate();
    }
    else{
        do
        {
            subir(150, 50);
            print(1, bc.angleActuator().ToString());
        } while (bc.angleActuator() > 290);
        dop
        {
            bc.turnActuatorDown(10);
            print(1, bc.angleActuator().ToString());
        } while (bc.angleBucket() > 330);
        
        bc.colorSens(Sensibilidade);
    }
};

setup();

while (true)
{
    condi();
    Obstaculo();
}