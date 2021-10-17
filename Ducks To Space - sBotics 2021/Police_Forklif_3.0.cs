//* Funções Return => VOID

    //print(n,msg)
    void print(int n, string msg){bc.printLCD(n, msg);}

    void printClear(){bc.ClearConsole();}

    //delay(n)
    void delay(int n){bc.wait(n);}

    //motor(left,right)
    void motor(int left, int right){bc.onTF(left, right);}

    //rot(speed,angle)
    void rot(int speed, float angle){bc.onTFRot(speed, angle);}

    //led("cor do led")
    void led(string color){bc.turnLedOn(color);}

    //led("cor do led")
    void ledRGB(int a, int b, int c){bc.turnLedOn(a, b, c);}

    //ledOff()
    void ledOff(){bc.turnLedOff();}

    //rstTimer()
    void rstTimer(){bc.resetTimer();}

    //descerGarra(speed, tempo) //velocidade max = 150
    void descerGarra(int speed, int time){bc.ActuatorSpeed(speed); bc.ActuatorDown(time);}
        
    //descerGarra(speed, tempo)
    void subirGarra(int speed, int time) {bc.ActuatorSpeed(speed); bc.ActuatorUp(time);}
        
    //mover atuador(tempo)  CIMA
    void turnUp(int time){bc.TurnActuatorUp(time);}

    //mover atuador(tempo)  BAIXO
    void turnDown(int time){bc.TurnActuatorDown(time);}



//* Funções Return => VALUE

    //cor(sensor)  1  2  3  4  5
    string cor(int sensor){return bc.returnColor(sensor - 1);}

    //luz(sensor)
    float luz(int sensor){return bc.lightness(sensor - 1);}

    //corB(sensor)
    float corB(int sensor){return bc.returnBlue(sensor - 1);}

    //corG(sensor)
    float corG(int sensor){return bc.returnGreen(sensor - 1);}

    //corR(sensor)
    float corR(int sensor){return bc.returnRed(sensor - 1);}

    //compass()
    float compass(){return bc.Compass();}

    //sonar(sensor)
    float sonar(int sensor){return bc.distance(sensor);}
        //sonar(0)  => CIMA
        //sonar(1)  => MEIO
        //sonar(2)  => BAIXO

    //inclinação
    float inclina(){return bc.Inclination();}

    //Timer()
    int timer(){return bc.timer();}


//* Variáveis 

    int velocidade = 200; 
    int Sensibilidade = 25; //padrão 25
    float linha = 40; 

//* Inicialização

    int BallDistance1;
    int BallDistance2;
    int WallDistance1;
    int WallDistance2;

    int numeroBolinhas = 0;
    int numeroBolinhasPretas = 0;
    int numeroBolinhasBrancas = 0;

    float pos1 = 0;
    float pos2 = 0;
    float pos3 = 0;

    float anguloCarrinho;
    float anguloParaVoltar;
    float anguloInicial;

    float distanciaAtéBolinha = 0;
    float distanciaAtéVitima = 0;
    float distanciaAtéVitimaMorta = 0;

    double tempoDeIda = 0;

    bool resgatePos1 = false;
    bool resgatePos2 = false;
    bool resgatePos3 = false;
    bool resgatePos4 = false;


    bool doisVerdes = false;
    bool resgateDeRe = false;
    bool leftturn = false;
    bool vermelhoSaida = false;
    bool Sala4x3 = false;

    bool flagPegarVitima = true;
    bool flagAlarmeFalso = false;


//* Função Principal
void Main(){

    //!setup//////////// 
        bc.colorSens(Sensibilidade);

        //subir garra no inicio
        Subir();
        print(2, luz(1).ToString() + "  " + luz(2).ToString() + "  " +  luz(3).ToString() + "  " +  luz(4).ToString() + "  " + luz(5).ToString());

    //!loop////////////
        while(true){	 

            LineFollow();
            VerifLinhaCinza();
            Verde();
            ObstaculoOuKit();
            VerifSuperGap(1300);
            Gangorra();
          
        }
}

void Main2(){
    //!setup//////////// 
        vermelhoSaida = true;
        
    //!loop////////////

    while(true){
        LineFollow();
        Verde();
        ObstaculoOuKit();
        VerifSuperGap(1300);
        LinhaDeChegada();
        Gangorra();
    }

}

void Teste(){
    
    //COLOCAR CODIGO TESTE

    //!////////////////////////
    motor(0,0);
    delay(1000000);
}


//* Funções para Garra
//?Descer(Angulo para Descer, Velocidade); //Descer(); para descer a garra totalmente
void Descer(int a = 1, int b = 150){
    
    while(bc.AngleActuator() > a){   
        descerGarra(b, 1);
    } 
}

//?Subir(Angulo para Subir, Velocidade); //Subir(); para subir a garra totalmente
void Subir(int a = 88, int b = 150){
    
    while(bc.AngleActuator() < a){   
        subirGarra(b, 1);
    }
}


//* Funções para seguir linha
void LineFollow(){
    saida:
    
    //Frente
    motor(velocidade,velocidade);

    //Virar para Esquerda
    if((luz(5) < linha || luz(4) < linha) && (luz(1) > linha) && (cor(1) != "VERMELHO" || cor(5) != "VERMELHO")){
        led("vermelho");

        rstTimer();
        while(cor(3) != "BRANCO"){
            motor(300,300);
            delay(100);

            if(timer() >= 1000){
                goto saida;
            }
        }

        rstTimer();

        //virar até encontrar a linha
        while(cor(3) != "PRETO"){
            motor(1000,-1000);

            if(luz(1) <= linha || luz(2) <= linha){
                rot(1000, 5);
                
                goto saida;
            }

            //Timer para correção
            if(timer() >= 2000){
                
                rstTimer();
                
                while(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO"){
                    motor(-1000,1000);

                    //Timer para correção
                    if(timer() >= 3000){
                        rot(1000, -20);

                        while(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO"){
                            motor(-100,-100);
                        }

                        motor(-200,-200);
                        delay(200);

                        rot(1000,-5);

                        break;
                    }
                }
                break;
            }
        }

        ledOff();
    }        

    //Virar para Direita
    if((luz(1) < linha || luz(2) < linha) && (luz(5) > linha) && (cor(1) != "VERMELHO" || cor(5) != "VERMELHO")){
        led("azul");
    
        rstTimer();
        while(cor(3) != "BRANCO"){
            motor(300,300);
            delay(100);//wip

            if(timer() >= 1000){
                goto saida;
            }
        }

        rstTimer();

        //virar até encontrar a linha
        while(cor(3) != "PRETO"){
            motor(-1000,1000);

            if(luz(4) <= linha || luz(5) <= linha){
                rot(1000, -5);
                
                goto saida;
            }

            //Timer para correção
            if(timer() >= 2000){
                
                rstTimer();
                
                while(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO"){
                    motor(1000,-1000);

                    //Timer para correção
                    if(timer() >= 3000){
                        rot(1000, 20);

                        while(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO"){
                            motor(-100,-100);
                        }

                        motor(-200,-200);
                        delay(200);

                        rot(1000,5);

                        break;
                    }
                }
                break;
            }
        }

        ledOff();
    }

    VerifLinhaCinza();

    //vermelho fora da pista
    if(!vermelhoSaida && ((corB(1) < corR(1) && corG(1) < corR(1) && corR(1) > 30) || (corB(5) < corR(5) && corG(5) < corR(5) && corR(5) > 30)) ){
        ledRGB(200,80,80);
        print(0, "Sai da Pista, voltando...");
        
        while(cor(3) != "PRETO"){
            motor(-150,-150);
        }

        ledRGB(0,200,100);

        motor(-200,-200);
        delay(200);

        rot(1000, -10);

        printClear();
        ledOff();
    }

}

void Verde(){
    int tempoDeIdaVerde = 600;
    int tempoDeCondiVerde = 1000;
    int tempoDeVerificação = 510;

    float verde = 70;

    saida:

    //Verde no meio
    if(cor(3) == "VERDE"){
        led("verde");
        print(0, "VERDE meio");
        
        while(cor(3) == "VERDE"){
            motor(-300,-300);
        }
        motor(-300,-300);
        delay(200);

        rot(1000, 5);

        printClear();
        ledOff();
    }

    //Verde na Esquerda
    if (cor(5) == "VERDE" || cor(4) == "VERDE" || cor(5) == "AMARELO" || cor(4) == "AMARELO"){
        led("verde");
        print(0, "<= VERDE");

        rot(1000,-2);

        //verificar 2 verdes
        if((cor(1) == "VERDE" || cor(2) == "VERDE") || (cor(1) == "AMARELO" || cor(2) == "AMARELO")){
            print(1, "Dois Verdes");
            rot(1000,150);
            CorreçãoDeGiroReto();
            
            doisVerdes = true;
        }

        //não tem 2 verdes
        if(!doisVerdes){
           
            rot(1000,2);

            //ir reto até deixar de ver a linha
            rstTimer();
            while(cor(5) != "PRETO" && cor(4) != "PRETO" && cor(2) != "PRETO" && cor(1) != "PRETO"){        
                motor(50,50);  

                if(timer() >= 1000){
                    goto saida;
                }

            }

            CorreçãoDeGiroReto();

            motor(300, 300);
            delay(tempoDeIdaVerde);  //470

            rot(1000, -20);

            //girar ate achar a linha 
            while (cor(3) != "PRETO")
            {
                motor(1000, -1000);
            }

            CorreçãoDeGiroReto();

            motor(-1000, -1000);
            delay(300);

            //!Verif Quadrado ou Circulo Verde
            led("amarelo");
            
            rstTimer();
            while(timer() < tempoDeCondiVerde){
                motor(150,150);
                
                //Quadrado ou Circulo Verde Esquerda ativado
                if(cor(1) == "PRETO"){
                    print(2, timer().ToString() + "ms");

                    if(timer() < tempoDeVerificação){
                        CirculoVerdeEsquerda();
                    }else{
                        QuadroVerdeEsquerda();
                    }

                    break;
                }
            }
            print(2, timer().ToString() + "ms");

            ledOff();
        }     

        ledOff();
        doisVerdes = false;
    }

    //Verde na Direita
    if (cor(2) == "VERDE" || cor(1) == "VERDE" || cor(2) == "AMARELO" || cor(1) == "AMARELO"){
        led("verde");
        print(0, "VERDE =>");
        
        rot(1000,2);

        //verificar 2 verdes
        if((cor(4) == "VERDE" || cor(5) == "VERDE") || (cor(4) == "AMARELO" || cor(5) == "AMARELO")){
            print(1, "Dois Verdes");
            rot(1000,150);
            CorreçãoDeGiroReto();
            
            doisVerdes = true;
        }
        
        if(!doisVerdes){
            
            rot(1000,-2);

            rstTimer();
            while(cor(5) != "PRETO" && cor(4) != "PRETO" && cor(2) != "PRETO" && cor(1) != "PRETO"){
                motor(50,50);

                if(timer() >= 1000){
                    goto saida;
                }  
            }

            CorreçãoDeGiroReto();

            motor(300, 300);
            delay(tempoDeIdaVerde);  //470

            rot(1000, 20);
            
            //girar ate achar a linha 
            while (cor(3) != "PRETO")
            {
                motor(-1000, 1000);
            }

            CorreçãoDeGiroReto();

            motor(-1000, -1000);
            delay(300);

            //!Verif Quadrado ou Circulo Verde
            led("amarelo");

            rstTimer();
            while(timer() < tempoDeCondiVerde){
                motor(150,150);
                
                //Quadrado ou Circulo Verde Direita ativado
                if(cor(5) == "PRETO"){
                    print(2, timer().ToString() + "ms");

                    if(timer() < tempoDeVerificação){
                        CirculoVerdeDireita();
                    }else{
                        QuadroVerdeDireita();
                    }

                    break;
                }
            }
        
            printClear();
            ledOff();
        }

        printClear();
        ledOff();
        doisVerdes = false;
    }
}

void ObstaculoOuKit(){
    
    if (sonar(2) < 11 && sonar(0) < 17 && cor(6) != "CIANO"){
        
        led("amarelo");
        print(0, "OBSTÁCULO");

        motor(-200,-200);
        delay(200);

        motor(0,0);

        //correção de giro 45°
        while(true){
            // NORTE 
            if ((compass() >= 0 && compass() < 45) || (compass() > 315 && compass() <= 360)){            
                print(1, "NORTE ATIVADO");
                rot(1000, 15);
                
                while (compass() < 40){
                    motor(-1000, 1000);
                } 
                break;
            }
            // OESTE
            else if(compass() > 225 && compass() < 315){
                print(1, "OESTE ATIVADO");
                rot(1000, 15);

                while (compass() < 310){
                    motor(-1000, 1000);
                } 
                break;
            }
            // SUL
            else if(compass() > 135 && compass() < 225){
                print(1, "SUL ATIVADO");
                rot(1000, 15);
                
                while (compass() < 220){
                    motor(-1000, 1000);
                } 
                break;
            }  
            // LESTE
            else if(compass() > 45 && compass() < 135){   
                print(1, "LESTE ATIVADO");
                rot(1000, 15);

                while (compass() < 130){
                    motor(-1000, 1000);
                } 
                break;
            } 
        }

        //condição 1
        rstTimer();
        while (true){

            motor(200, 200); //150 150

            if (luz(5) < linha || luz(1) < linha){
                
                print(1,"condi 1");
                
                motor(1000, 1000);
                delay(650);
                
                rot(500, 40);
                CorreçãoDeGiroReto();

                while(!bc.touch(0)){
                    motor(-200,-200);
                }
                
                break;
            }

            if (timer() >= 1000){
                
                motor(-200, -200);
                delay(1000);

                CorreçãoDeGiroReto();
                rot(1000,-40);
                
                rstTimer();

                //condição 2
                while (true){
                    motor(200, 200);//150 150

                    if (luz(5) < linha || luz(1) < linha){
                        print(1,"condi 2");
                        
                        motor(1000, 1000);
                        delay(550);
                        
                        rot(500, -50);
                        CorreçãoDeGiroReto();

                        while(!bc.touch(0)){
                            motor(-200,-200);
                        }
                        
                        led("ciano");
                        rstTimer();
                        while(cor(3) != "PRETO"){
                            motor(-1000,1000);

                            if(timer() >= 2000){
                                rstTimer();

                                while(cor(3) != "PRETO"){
                                    motor(1000,-1000);
                                    
                                    if(timer() >= 4000){
                                        rot(1000,30);

                                        CorreçãoDeGiroReto();
                                        
                                        break;
                                    }
                                }

                                break;
                            }
                        }

                        break;
                    }                   
                    
                    if (timer() >= 1200){   
                        print(1,"condi 3");
                        
                        rot(1000, 30);
                        CorreçãoDeGiroReto();
                        
                        motor(500,500);
                        delay(1000); //ajustavel

                        rot(1000, 70);

                        while(luz(3) >= linha){
                            motor(100,100);
                        }

                        motor(1000, 1000);
                        delay(500);

                        rot(1000, -70);

                        CorreçãoDeGiroReto();
                                        
                        while(!bc.touch(0)){
                            motor(-100,-100);
                        }

                        motor(0,0);
                        break;
                    }
                }

                break;
            }
        }

        printClear();
        ledOff();

    }
    //Pegar kit de Resgate
    else if(sonar(2) < 11 && cor(6) == "CIANO"){
        led("ciano");
        print(0, "Kit de Resgate IDENTIFICADO");

        voltar3:

        while(sonar(2) < 25){
            motor(-1000,-1000);
        }
        motor(0,0);

        Descer();

        bc.OpenActuator();

        motor(1000,1000);
        delay(1000);
        motor(0,0);

        Subir();

        bc.CloseActuator();

        if(sonar(2) < 11 && cor(6) == "CIANO"){
            goto voltar3;
        }

        while(cor(3) != "PRETO"){
            motor(-1000,-1000);
        }
        motor(0,0);
        

        ledOff();
        print(0, "Kit de Resgate PEGO");
    }
}

void VerifSuperGap(float tempoParaSuperGap){

    if(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
        
        rstTimer();
        print(1, "SuperGap = ...");

        while(true){
            
            motor(velocidade,velocidade);

            VerifLinhaCinza();
            if(vermelhoSaida == true){
                LinhaDeChegada();
            }

            //condição de break
            if(cor(1) != "BRANCO" || cor(2) != "BRANCO" || cor(3) != "BRANCO" || cor(4) != "BRANCO" || cor(5) != "BRANCO"){
                motor(0,0);
                break;
            }

            //ativar super gap
            if(timer() >= tempoParaSuperGap){
                
                led("branco");
                print(1, "SuperGap = Ativado");

                SuperGap();
                
                break;
            }

            if(sonar(2) < 15){
                motor(0,0);
                break;
            }
        }

        printClear();
        ledOff();
    }
}

void SuperGap(){

    rstTimer();

    //verificando linha na Esquerda
    while(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
        motor(-1000,1000);

        //nao tem linha na esquerda
        if(timer() >= 2500){//tempo ajustavel
            
            rstTimer();

            //verificando linha na Direita
            while(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                motor(1000,-1000);

                //nao tem linha na Direita
                if(timer() >= 5000){//tempo ajustavel
                    
                    rot(1000,60);//angulo ajustavel

                    //voltar para a linha de costas
                    while(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                        motor(-200,-200);   

                        VerifLinhaCinza();
                    }

                    motor(-200,-200);
                    delay(100);

                    break;
                }
            }

            break;
        }
    }
}

void VerifLinhaCinza(){


    if( (corB(2) > corR(2)) && (corB(2) > corG(2)) || (corB(4) > corR(4)) && (corB(4) > corG(4))){
        
        ledRGB(100, 100, 100);
       
        CorreçãoDeGiroReto();
        
        //entrar na sala de Resgate
        rstTimer();
        while(sonar(1) > 400){
            motor(300,300);

            if(timer() >= 500){
                break;
            }
        }
        
        motor(300,300);
        delay(700);

        //verifica sala 4x3
        if(sonar(0) >= 325){
            print(0, "Sala 4x3");
            Sala4x3 = true;
        }
        //verifica sala 3x4
        else{
            print(0, "Sala 3x4");
            Sala4x3 = false;
        }

        InitResgate();
    }

}

void LinhaDeChegada(){
    
    //Saiu do Resgate --> vermelho da Saida 

    if(cor(2) == "VERMELHO" || cor(3) == "VERMELHO" || cor(4) == "VERMELHO"){
        print(0, "LINHA DE CHEGADA");
        
        motor(300,300);
        delay(500);
        ledRGB(250,80,100);
        
        int a = (int)(timer()/1000);
        
        rstTimer();
        while(true){
            
            motor(0,0);
            a = (int)(timer()/1000);
            print(1, a.ToString() + "...");

            if(timer() >= 10000){
                break;
            }
        }
    }
}

void Gangorra(){
    if(inclina() >= 335 && inclina() < 342){
        delay(5);
        
        if(inclina() >= 335 && inclina() < 342){
            print(1, "Gangorra = Ativado...");
            anguloInicial = compass();
            print(0,anguloInicial.ToString());
            
            rstTimer();
            CorreçãoDeGiroReto();
            while(true){
                motor(velocidade,velocidade);

                if(inclina() > 0 && inclina() < 10){
                    
                    motor(-300,-300);
                    delay(300);

                    while(inclina() >= 10 && inclina() < 30){
                        motor(100,100);
                    }
                    break;
                }

                if(anguloInicial - compass() > 10 || compass() - anguloInicial  > 10){
                    CorreçãoDeGiroReto();
                }
            }
        }
        
    }

}


//* Funções para Função Verde
void QuadroVerdeEsquerda(){

    print(0, "Quadrado Esquerda ATIVADO");

    //impulso
    motor(300,300);
    delay(500);

    //virar até encontrar a linha
    rot(1000,40);
    led("amarelo");
    rstTimer();
    while(cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" ){
        motor(-1000,1000);

        if(timer() >= 2000){
            break;
        }
    }
    ledOff();
        
    //se esta em um gap: condi 2 ou condi 3
    if(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO"){
        CorreçãoDeGiroReto();
        print(1, "Contornando Gap...");

        rot(1000,30);

        motor(300,300);
        delay(1100);

        CorreçãoDeGiroReto();

        motor(300,300);
        delay(200);

        rstTimer();
        while(cor(3) != "PRETO"){
            motor(1000,-1000);

            if(timer() >= 1000){
                rstTimer();

                while(cor(3) != "PRETO"){
                    motor(-1000,1000);

                    if(timer() >= 2000){
                        rot(1000,-20);

                        CorreçãoDeGiroReto();
                        break;
                    }
                }

                break;
            }
        }

        if(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO"){
            motor(-300,-300);
            delay(650);

            rot(1000,80);
            CorreçãoDeGiroReto();

            motor(300,300);
            delay(700);
        }
    
    }
    
    //se esta em uma linha: condi 1, condi 2 ou condi 3
    else{
        
        //pegar espaço
        CorreçãoDeGiroReto();
        motor(-300,-300);
        delay(200);

        //Verif linha na Esquerda: Condi 1
        rstTimer();
        while(true){
            motor(150,150);

            //se linha na Esquerda: Condi 1 ATIVADO
            if(cor(5) == "PRETO"){
                print(1, "Linha na Esquerda: Condição 1");

                //impulso
                motor(300,300);
                delay(500);

                //90 graus para Direita
                rot(1000,-50);
                CorreçãoDeGiroReto();

                //pegar espaço
                motor(-300,-300);
                delay(100);

                //ir para trás ate ver verde
                do{
                    motor(-100,-100);
                }while(cor(1) != "VERDE" && cor(2) != "VERDE" && cor(3) != "VERDE" && cor(4) != "VERDE" && cor(5) != "VERDE");
                
                //impulso
                motor(300,300);
                delay(100);

                break;
            }

            //Não tem linha na Esquerda => Condi 2 ou Condi 3
            if(timer() >= 1000){
                
                CorreçãoDeGiroReto();
                print(1, "Contornando...");

                //indo ate a linha na frente
                rot(1000,40);
                motor(300,300);
                delay(850);

                CorreçãoDeGiroReto();

                //zig-zag
                rstTimer();
                while(cor(3) != "PRETO"){
                    motor(-1000,1000);

                    if(timer() >= 1000){
                        rstTimer();

                        while(cor(3) != "PRETO"){
                            motor(1000,-1000);

                            if(timer() >= 2000){
                                rot(1000,20);

                                CorreçãoDeGiroReto();
                                break;
                            }
                        }

                        break;
                    }
                }

                //verif linha na frente: condi 3
                if(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO"){
                    print(1, "Linha na Direita: Condição 3");
                    
                    motor(-300,-300);
                    delay(400);

                    rot(1000,80);
                    CorreçãoDeGiroReto();

                    motor(300,300);
                    delay(700);
                }

                //linha na frente: condi 2
                else{
                    print(1, "Linha na Frente: Condição 2");

                    //zig-zag
                    rstTimer();
                    while(cor(3) != "PRETO"){
                        motor(-1000,1000);

                        if(timer() >= 1000){
                            rstTimer();

                            while(cor(3) != "PRETO"){
                                motor(1000,-1000);

                                if(timer() >= 2000){
                                    rot(1000,20);

                                    CorreçãoDeGiroReto();
                                    break;
                                }
                            }

                            break;
                        }
                    }

                    //pegar espaço
                    motor(-300,-300);
                    delay(100);

                    //ir para trás ate ver verde
                    do{
                        motor(-100,-100);
                    }while(cor(1) != "VERDE" && cor(2) != "VERDE" && cor(3) != "VERDE" && cor(4) != "VERDE" && cor(5) != "VERDE");

                    motor(300,300);
                    delay(100);
                }

                break;
            }
        }

    }

}

void QuadroVerdeDireita(){

    print(0, "Quadrado Direita ATIVADO");

    //impulso
    motor(300,300);
    delay(500);

    //virar para achar linha
    rot(1000,-40);
    led("amarelo");
    rstTimer();
    while(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO" ){
        motor(1000,-1000);

        if(timer() >= 2000){
            break;
        }
    }
    ledOff(); 
    
    //se tiver em cima de um gap: condi 2 ou condi 3
    if(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO"){
        CorreçãoDeGiroReto();
        print(1, "Contornando Gap...");

        rot(1000,-30);

        motor(300,300);
        delay(1100);

        CorreçãoDeGiroReto();

        motor(300,300);
        delay(200);

        rstTimer();
        while(cor(3) != "PRETO"){
            motor(-1000,1000);

            if(timer() >= 1000){
                rstTimer();

                while(cor(3) != "PRETO"){
                    motor(1000,-1000);

                    if(timer() >= 2000){
                        rot(1000,20);

                        CorreçãoDeGiroReto();
                        break;
                    }
                }

                break;
            }
        }

        if(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO"){
            motor(-300,-300);
            delay(650);

            rot(1000,-80);
            CorreçãoDeGiroReto();

            motor(300,300);
            delay(700);
        }
    
    }
    
    //se esta em uma linha: condi 1, condi 2 ou condi 3
    else{
        //pegar espaço
        CorreçãoDeGiroReto();
        motor(-300,-300);
        delay(300);

        //Verif linha na Direita: Condi 1
        rstTimer();
        while(true){
            motor(150,150);

            //se linha na Esquerda: Condi 1 ATIVADO
            if(cor(1) == "PRETO"){
                print(1, "Linha na Direita: Condição 1");
                
                //impulso
                motor(300,300);
                delay(500);

                //90 graus para Direita
                rot(1000,50);
                CorreçãoDeGiroReto();

                //pegar espaço
                motor(-300,-300);
                delay(100);

                //ir para trás ate ver verde
                do{
                    motor(-100,-100);
                }while(cor(1) != "VERDE" && cor(2) != "VERDE" && cor(3) != "VERDE" && cor(4) != "VERDE" && cor(5) != "VERDE");

                //impulso
                motor(300,300);
                delay(100);
                
                break;
            }

            //Não tem linha na Direita => Condi 2 ou Condi 3
            if(timer() >= 1000){
                
                CorreçãoDeGiroReto();
                print(1, "Contornando...");

                //indo ate a linha na frente
                rot(1000,-40);
                motor(300,300);
                delay(850);

                CorreçãoDeGiroReto();

                //zig-zag
                rstTimer();
                while(cor(3) != "PRETO"){
                    motor(1000,-1000);

                    if(timer() >= 1000){
                        rstTimer();

                        while(cor(3) != "PRETO"){
                            motor(-1000,1000);

                            if(timer() >= 2000){
                                rot(1000,-20);

                                CorreçãoDeGiroReto();
                                break;
                            }
                        }

                        break;
                    }
                }

                //verif linha na frente: condi 3
                if(cor(1) == "BRANCO" && cor(2) == "BRANCO" && cor(3) == "BRANCO" && cor(4) == "BRANCO" && cor(5) == "BRANCO"){
                    print(1, "Linha na Esquerda: Condição 3");

                    //linha na Direita
                    motor(-300,-300);
                    delay(400);

                    rot(1000,-80);
                    CorreçãoDeGiroReto();

                    motor(300,300);
                    delay(700);
                }
                
                //linha na frente: condi 2
                else{
                    print(1, "Linha na Frente: Condição 2");

                    //zig-zag
                    rstTimer();
                    while(cor(3) != "PRETO"){
                        motor(1000,-1000);

                        if(timer() >= 1000){
                            rstTimer();

                            while(cor(3) != "PRETO"){
                                motor(-1000,1000);

                                if(timer() >= 2000){
                                    rot(1000,-20);

                                    CorreçãoDeGiroReto();
                                    break;
                                }
                            }

                            break;
                        }
                    }

                    //pegar espaço
                    motor(-300,-300);
                    delay(100);

                    //ir para trás ate ver verde
                    do{
                        motor(-100,-100);
                    }while(cor(1) != "VERDE" && cor(2) != "VERDE" && cor(3) != "VERDE" && cor(4) != "VERDE" && cor(5) != "VERDE");

                    motor(300,300);
                    delay(100);
                }

                break;
            }
        }

    }

}

void CirculoVerdeEsquerda(){
    print(0, "Circulo Esquerda ATIVADO");

    //impulso
    motor(300,300);
    delay(750);

    //vira pra condição 1
    rot(1000, 70);
    CorreçãoDeGiroReto();

    //pegar espaço  
    motor(-300,-300);
    delay(300);

    //Verif Condi 1, Condi 2, Condi 3
    rstTimer();
    while(true){
        motor(150,150);

        //Verif linha na Esquerda: Condi 1
        if(cor(5) == "PRETO"){
            print(1, "linha na Esquerda: Condição 1");
            
            //impuslo
            motor(300,300);
            delay(400);

            //Virar 90° para Esquerda
            rot(1000,-50);
            CorreçãoDeGiroReto();

            //pegar espaço
            motor(-300,-300);
            delay(100);
            
            //Ir de Costas ate ver verde
            do{
                motor(-100,-100);
            }while(cor(1) != "VERDE" && cor(2) != "VERDE" && cor(3) != "VERDE" && cor(4) != "VERDE" && cor(5) != "VERDE");

            motor(300,300);
            delay(100);

            break;
        }

        if(timer() >= 1500){
            CorreçãoDeGiroReto();
            print(1, "Contornando...");

            rot(1000,40);

            motor(300,300);
            delay(1000);

            CorreçãoDeGiroReto();

            rstTimer();
            while(cor(3) != "PRETO"){
                motor(1000,-1000);

                if(timer() >= 1000){
                    rstTimer();

                    while(cor(3) != "PRETO"){
                        motor(-1000,1000);

                        if(timer() >= 2000){
                            rot(1000,-20);

                            CorreçãoDeGiroReto();
                            break;
                        }
                    }

                    break;
                }
            }

            //verif linha na frente
            if(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                
                //linha na Direita
                motor(-300,-300);
                delay(800);

                rot(1000,80);
                CorreçãoDeGiroReto();

                motor(300,300);
                delay(700);
            }

            break;
        }
    
    }

}

void CirculoVerdeDireita(){
    
    print(0, "Circulo Direita ATIVADO");

    //impulso
    motor(300,300);
    delay(750);

    //vira pra condição 1
    rot(1000, -70);
    CorreçãoDeGiroReto();

    //pegar espaço
    motor(-300,-300);
    delay(300);

    //Verif Condi 1, Condi 2, Condi 3
    rstTimer();
    while(true){
        motor(150,150);

        //Verif linha na Esquerda: Condi 2
        if(cor(1) == "PRETO"){
            print(1, "linha na Direita: Condição 1");

            //impuslo
            motor(300,300);
            delay(400);

            //Virar 90° para Direita
            rot(1000,50);
            CorreçãoDeGiroReto();

            //pegar espaço
            motor(-300,-300);
            delay(100);

            //Ir de Costas ate ver verde
            do{
                motor(-100,-100);
            }while(cor(1) != "VERDE" && cor(2) != "VERDE" && cor(3) != "VERDE" && cor(4) != "VERDE" && cor(5) != "VERDE");

            motor(300,300);
            delay(100);

            break;
        }

        if(timer() >= 1500){
            CorreçãoDeGiroReto();
            print(1, "Contornando...");

            rot(1000,-40);

            motor(300,300);
            delay(1000);

            CorreçãoDeGiroReto();

            rstTimer();
            while(cor(3) != "PRETO"){
                motor(-1000,1000);

                if(timer() >= 1000){
                    rstTimer();

                    while(cor(3) != "PRETO"){
                        motor(1000,-1000);

                        if(timer() >= 2000){
                            rot(1000,20);

                            CorreçãoDeGiroReto();
                            break;
                        }
                    }

                    break;
                }
            }

            //verif linha na frente
            if(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                
                //linha na Direita
                motor(-300,-300);
                delay(800);

                rot(1000,-80);
                CorreçãoDeGiroReto();

                motor(300,300);
                delay(700);
            }

            break;
        }
    }
}


//* Funções Principais para resgate
void InitResgate(){
    print(1, "  ");
    motor(300,300);
    delay(300);

    CorreçãoDeGiroReto();

    //90° esquerda
    rot(1000,-80);
    CorreçãoDeGiroReto();

    //vai reto ate chegar na parede
    while(true){
        motor(300,300);
        if(sonar(0) < 26){
            break;
        }

        //ara de resgate 1
        if((sonar(0) - sonar(2) >= 20) && sonar(0) < 120 && cor(6) == "PRETO"){
            CorreçãoDeGiroReto();

            EntregarKit();
            resgatePos1 = true;

            break;
        }
    }

    //!VERIFICAR RESGATES
        print(0, "Entregar KitDeResgate =" + "  " + bc.HasRescueKit().ToString());

        //verificação de outros resgates (pos 2, 3, 4)
        if(resgatePos1 == false){
            rot(1000,80);
            CorreçãoDeGiroReto();

            //impulso para nao ver Resgate pos 3
            if(Sala4x3 && !bc.HasRescueKit()){
                BallDistance1 = 220;
                while(sonar(0) > 270){
                    motor(300,300);
                }
            }
            else if(!bc.HasRescueKit()){
                BallDistance1 = 160;
                while(sonar(0) > 181){
                    motor(300,300);
                }
            }
        
            //Verif Resgate 2
            while(true){ 
                //ir pra frente
                motor(300,300); 

                //verif vitima no lado caso nao tenha Kit
                if(!bc.HasRescueKit() && !bc.HasVictim() && sonar(1) < BallDistance1 && sonar(1) > 17){
                    led("verde");

                    InitBolinhaNoLado();

                    //pegar bolinha   
                    if((sonar(0) - sonar(2)) >= 15 && sonar(2) < 40){               
                        led("verde");
                        PegarBolinha();
                        led("vermelho");
                    }

                    else{
                        rstTimer();

                        //procurar vitima de um lado
                        while((sonar(0) - sonar(2)) <= 20 || sonar(2) > 30){
                            motor(-1000,1000);

                            if(timer() >= 1500){
                                rstTimer();
                                //procurar vitima no outro lado
                                while((sonar(0) - sonar(2)) <= 20 || sonar(2) > 30){
                                    motor(1000,-1000);

                                    if(timer() >= 3000){
                                        led("vermelho");
                                        
                                        CorreçãoDeGiroReto();
                                        
                                        //voltar para parede
                                        while(!bc.touch(0)){
                                            motor(-1000,-1000);

                                            if(cor(1) == "VERDE" && cor(5) == "VERDE"){
                                                motor(200,200);
                                                delay(1400);
                                                break;
                                            }
                                        }
                                        
                                        rot(1000,-80); 
                                        CorreçãoDeGiroReto();  

                                        break;

                                    }
                                }

                                leftturn = true;
                                //rot(1000, -10);
                                break;
                            }
                        }

                        if(!leftturn){rot(1000, 10);}

                        led("verde");
                        PegarBolinha();
                        led("vermelho");

                        CorreçãoDeGiroReto();

                    }



                    //ta com a bolinha?
                    if(bc.HasVictim()){
                        
                        led("verde");
                        
                        //voltar para parede           
                        while(!bc.touch(0)){ 
                            motor(-300,-300); //?velocidade ajustável (voltar para parede)

                            if(cor(1) == "VERDE" && cor(5) == "VERDE"){
                                motor(200,200);
                                delay(1400);
                                break;
                            }
                        }

                        //impulso 
                        motor(300,300);
                        delay(600);

                        //correção de Giro 90°
                        rot(1000,-70);
                        CorreçãoDeGiroReto();
                    }
                    else{
                        
                        CorreçãoDeGiroReto();
                    }
                }   

                //verif resgate pos 2
                if((sonar(0) - sonar(2) >= 20) && sonar(0) < 120 && cor(6) == "PRETO"){
                    motor(300,300);
                    delay(100);
                    
                    CorreçãoDeGiroReto();
                    
                    resgatePos2 = true;
                    break;
                }

                //verif parede 2
                if(sonar(0) < 20 && sonar(2) < 5){     
                    motor(-300,-300);
                    delay(300);
                    
                    rot(1000,80);

                    CorreçãoDeGiroReto();
                    
                    break;
                }   
            }

            //Verif Resgate 3
            if(!resgatePos1 && !resgatePos2){
                
                //seguindo na 2° parede
                while(true){
                    
                    motor(300,300);

                    //verif vitima no lado caso nao tenha Kit
                    if(!bc.HasRescueKit() && !bc.HasVictim() && sonar(1) < BallDistance1 && sonar(1) > 17){
                        led("verde");

                        InitBolinhaNoLado();

                        //pegar bolinha   
                        if((sonar(0) - sonar(2)) >= 17 && sonar(2) < 40){               
                            led("verde");
                            PegarBolinha();
                            led("vermelho");
                        }

                        //ta com a bolinha?
                        if(bc.HasVictim()){
                            
                            led("verde");
                            
                            //voltar para parede           
                            while(!bc.touch(0)){ 
                                motor(-300,-300); //?velocidade ajustável (voltar para parede)

                                if(cor(1) == "VERDE" && cor(5) == "VERDE"){
                                    motor(200,200);
                                    delay(1400);
                                    break;
                                }
                            }

                            //impulso 
                            motor(300,300);
                            delay(600);

                            //correção de Giro 90°
                            rot(1000,-70);
                            CorreçãoDeGiroReto();
                        }
                    }   

                    //Verif Resgate 3
                    if((sonar(0) - sonar(2) >= 20) && sonar(0) < 120 && cor(6) == "PRETO"){
                        CorreçãoDeGiroReto();
                        
                        resgatePos3 = true;
                        break;
                    }

                    //verif parede 3
                    if(sonar(0) < 20 && sonar(2) < 5){     
                        motor(-300,-300);
                        delay(300);
                        
                        rot(1000,80);
                        CorreçãoDeGiroReto();

                        break;
                    }

                }
            }
        
            //Verif Resgate 4
            if(!resgatePos1 && !resgatePos2 && !resgatePos3){
                //seguindo na 3° parede
                while(true){
                    motor(300,300);

                    //resgate pos3
                    if((sonar(0) - sonar(2) >= 20) && sonar(0) < 120 && cor(6) == "PRETO"){
                        CorreçãoDeGiroReto();
                        
                        resgatePos4 = true;
                        break;
                    }
                } 
            }
            
        }
    
    //!ATIVAR RESGATES
        //*RESGATE POSIÇÃO 1
        if(resgatePos1 && !resgatePos2 && !resgatePos3){
            
            print(0, "Resgate 1 ATIVADO");
            ResgatePosição1();
        }

        //*RESGATE POSIÇÃO 2
        if(!resgatePos1 && resgatePos2 && !resgatePos3){
            
            print(0, "Resgate 2 ATIVADO");
            ResgatePosição2();
        }

        //*RESGATE POSIÇÃO 3
        if(!resgatePos1 && !resgatePos2 && resgatePos3){
            
            print(0, "Resgate 3 ATIVADO");
            ResgatePosição3();
        }

        //*RESGATE POSIÇÃO 4
        if(!resgatePos1 && !resgatePos2 && !resgatePos3 && resgatePos4){
            
            print(0, "Resgate 4 ATIVADO");
            ResgatePosição4();
        }

}

void ResgatePosição1(){
    motor(0,0);
    led("amarelo");

    if(bc.HasRescueKit()){
        EntregarKit();
    }
    else if(bc.HasVictim()){
        EntregarBolinha();
    }

    if(Sala4x3){
        BallDistance1 = 322;
        BallDistance2 = 220;

        WallDistance1 = 240;
        WallDistance2 = 340;
        
    }
    else{
        BallDistance1 = 220; 
        BallDistance2 = 322;//verificar bolinha com sonar do lado
        
        WallDistance1 = 340;
        WallDistance2 = 240; //verificar parede com sonar da Frente
    }

    CorreçãoDeGiroReto();

    ResgateDeRé();
}

void ResgatePosição2(){
    motor(0,0);
    led("amarelo");

    if(bc.HasRescueKit()){
        EntregarKit();
    }else if(bc.HasVictim()){
        EntregarBolinha();
    }

    if(Sala4x3){
        BallDistance1 = 220;
        BallDistance2 = 322;

        WallDistance1 = 340; 
        WallDistance2 = 240; 
    }else{
        BallDistance1 = 322;
        BallDistance2 = 220;

        WallDistance1 = 240;
        WallDistance2 = 340; 
    }

    CorreçãoDeGiroReto();

    ResgateDeRé();
}

void ResgatePosição3(){
    motor(0,0);
    led("amarelo");

    if(bc.HasRescueKit()){
        EntregarKit();
    }else if(bc.HasVictim()){
        EntregarBolinha();
    }

    if(Sala4x3){
        BallDistance1 = 322;
        BallDistance2 = 220;

        WallDistance1 = 240;
        WallDistance2 = 340;
        
    }else{
        BallDistance1 = 220; 
        BallDistance2 = 322;//verificar bolinha com sonar do lado
        
        WallDistance1 = 340;
        WallDistance2 = 240; //verificar parede com sonar da Frente
    }

    CorreçãoDeGiroReto();

    ResgateDeRé();
}

void ResgatePosição4(){
    motor(0,0);
    led("amarelo");

    if(bc.HasRescueKit()){
        EntregarKit();
    }else if(bc.HasVictim()){
        EntregarBolinha();
    }

    if(Sala4x3){
        BallDistance1 = 220;
        BallDistance2 = 322;

        WallDistance1 = 340;
        WallDistance2 = 240;
    }else{
        BallDistance1 = 322;
        BallDistance2 = 220;

        WallDistance1 = 240;
        WallDistance2 = 340;
        
    }

    CorreçãoDeGiroReto();

    ResgateDeRé();
}


void ResgateDeRé(){
    resgateDeRe = true;

    voltarProcurarBolinha: 
    led("amarelo");
    
    //começar resgatar de ré
    while(numeroBolinhas < 3){
        
        //ir de costas
        motor(-300,-300); //?velocidade ajustável (ir de costas)

        //verif bolinha no lado
        if(sonar(1) < BallDistance1 && sonar(1) > 17){
            
            /*print(1, flagPegarVitima.ToString() + " | " +  sonar(0).ToString() 
            + "  " + (distanciaAtéVitimaMorta - 20).ToString() + "  " + (distanciaAtéVitimaMorta + 20).ToString() );*/

            //vitima viva 
            if(flagPegarVitima == true || !(sonar(0) > distanciaAtéVitimaMorta - 20 && sonar(0) < distanciaAtéVitimaMorta + 20)){
                InitBolinhaNoLado();
            
                //pegar bolinha de frente 
                if((sonar(0) - sonar(2)) >= 16 && sonar(2) < 40){               
                    
                    print(2, "Bolinha na Frente");
                    PegarBolinha();
                    
                }
                //perdeu a bolinha 
                else{
                    print(2, "perdi a vitima");
                    rstTimer();

                    //voltar para parede
                    while(bc.Touch(0) == false){
                        motor(-300,-300);
                        if(timer() >= 10000){
                            break;
                        }
                    }

                    rot(1000, -80);
                    CorreçãoDeGiroReto();
                }
                
                entregar:

                //ta com a bolinha
                if(bc.HasVictim()){
                    
                    led("verde");
                    print(2, "To com a Vitima, Indo Resgatar ...");
                    
                    //voltar para parede         
                    while(!bc.touch(0)){ 
                        motor(-300,-300); //?velocidade ajustável (voltar para parede)

                        if(sonar(0) >= WallDistance2){
                            break;
                        }
                    }

                    CorreçãoDeGiroReto();

                    //impulso 
                    motor(300,300);
                    delay(400);

                    //correção de Giro 90°
                    rot(1000,-70);
                    CorreçãoDeGiroReto();

                    //ir para resgate
                    while(luz(6) > 5 && sonar(0) > 25){
                        motor(300,300); //?velocidade ajustável (ir para resgate)
                    } 

                    EntregarBolinha();
                    
                    goto voltarProcurarBolinha;
                }
                //não ta com a bolinha 
                else{
                    
                    if(bc.HasVictim()){
                        goto entregar;

                    }else{
                        led("vermelho");

                        goto voltarProcurarBolinha;
                    }
                }
           
            }
            else{
                led("ciano");
                while(sonar(1) <= WallDistance2 - 20 ){
                    motor(-300,-300);
                }
                ledOff();
            }


        }

        //Resgate continua
        if(sonar(0) >= WallDistance1){
            motor(300,300);
            delay(200);
            
            rot(1000,-80);
            CorreçãoDeGiroReto();

            print(0,"Ignorando Resgate...");

            motor(-300,-300);
            delay(1000);

            print(0,"  ");

            ResgateDeRéContinua();
        }
    }

    SaindoDoResgate2();
}

void ResgateDeRéContinua(){
    
    voltarProcurarBolinha:
    led("amarelo");

    while(numeroBolinhas < 3){
        
        //ir de costas
        motor(-300,-300); //?velocidade ajustável (ir de costas)

        //verif bolinha no lado
        if(sonar(1) < BallDistance2 && sonar(1) > 17){
                
            InitBolinhaNoLado();
            print(1, flagAlarmeFalso.ToString());

            if(flagAlarmeFalso == false){
                //pegar bolinha na frente
                if((sonar(0) - sonar(2)) >= 16 && sonar(2) < 40){
                    led("verde");
                    PegarBolinha();
                    led("vermelho");
                }
                //perdeu a bolinha 
                else{
                    print(2, "perdi a vitima");
                    rstTimer();

                    //voltar para parede
                    while(bc.Touch(0) == false){
                        motor(-300,-300);
                        if(timer() >= 10000){
                            break;
                        }
                    }

                    rot(1000, -80);
                    CorreçãoDeGiroReto();
                }

                entregar:

                //ta com a bolinha
                if(bc.HasVictim()){
                    
                    rot(1000, -80);
                    CorreçãoDeGiroReto();

                    while(true){
                        motor(300,300);

                        //bolinha => desviar
                        if((sonar(0) - sonar(2)) >= 16 && sonar(2) < 40){
                            rot(1000, -45);

                            motor(300,300);
                            delay(1000);

                            rot(1000, 40);
                            CorreçãoDeGiroReto();
                        }
                        
                        //parede
                        if(sonar(0) < 25){
                            rot(1000, 80);
                            CorreçãoDeGiroReto();

                            //area de resgate
                            while(true){
                                motor(300,300);

                                if((sonar(0) - sonar(2) >= 20) && sonar(0) < 120 && cor(6) == "PRETO"){
                                    EntregarBolinha();

                                    break;
                                }
                            }

                            break;
                        }
                        
                        //saida
                        if(cor(3) == "VERDE" && sonar(0) > 400){
                            motor(-300,-300);
                            delay(300);

                            rot(1000, 80);
                            CorreçãoDeGiroReto();

                            //area de resgate
                            while(true){
                                motor(300,300);

                                if((sonar(0) - sonar(2) >= 20) && sonar(0) < 120 && cor(6) == "PRETO"){
                                    EntregarBolinha();

                                    break;
                                }
                            }

                            break;
                        }

                        //entrada
                        if((corB(2) > corR(2)) && (corB(2) > corG(2)) || (corB(4) > corR(4)) && (corB(4) > corG(4))){
                            motor(-300,-300);
                            delay(300);

                            rot(1000, 80);
                            CorreçãoDeGiroReto();

                            //area de resgate
                            while(true){
                                motor(300,300);

                                if((sonar(0) - sonar(2) >= 20) && sonar(0) < 120 && cor(6) == "PRETO"){
                                    EntregarBolinha();

                                    break;
                                }
                            }

                            break;
                        }
                    }

                    ResgateDeRé();
                
                }
                //não ta com a bolinha 
                else{
                    
                    if(bc.HasVictim()){
                        goto entregar;

                    }else{
                        led("vermelho");

                        goto voltarProcurarBolinha;
                    }
                } 
            
            }
            else{
                led("vermelho");
                print(2,"Saindo Faltando Bolinha...");

                motor(300,300);
                delay(1000);

                SaindoDoResgate2();
            }
        }
    
        //Saindo Do Resgate faltando vitima
        if((bc.touch(0) && sonar(0) > WallDistance2) || (sonar(0) > WallDistance2 && sonar(0) < 500)){
            
            led("vermelho");
            print(2,"Saindo Faltando Bolinha...");

            SaindoDoResgate2();
            break;
        }
    }
    
    SaindoDoResgate2();
}


void SaindoDoResgate2(){
    printClear();
    
    //180° 
    rot(1000, 170);
    CorreçãoDeGiroReto();

    VoltarVerificaçãoDeSaida:
    print(0,"Procurando Saida...");
    led("amarelo");

    //verif saida com sonar do lado
    while(true){
        motor(300,300);

        //Achou saida/entrada?
        if(sonar(1) >= 600){
            print(0, "Saida ?...");
            led("verde");
            
            motor(300,300);
            delay(500);
            
            rot(1000,80);
            CorreçãoDeGiroReto();

            motor(-300,-300);
            delay(200);

            rstTimer(); 
            //Verificação de saida com cor
            while(true){
                motor(200,200);

                //SAIDA !!!!
                if(cor(3) == "VERDE"){
                    print(0, "Saida Encontrada");
                    
                    while(true){
                        motor(300,300);

                        if(cor(1) == "PRETO" || cor(2) == "PRETO" || cor(3) == "PRETO" || cor(4) == "PRETO" || cor(5) == "PRETO"){
                            break;
                        }

                        if(sonar(1) > 9000){
                            motor(-300,-300);
                            delay(200);
                            motor(0,0);

                            break;
                        }
                    }

                    led("ciano");
                    if(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                        rstTimer();
                        while(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                            motor(-1000,1000);

                            if(timer() >= 2000){
                                rstTimer();

                                while(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                                    motor(1000,-1000);
                                    
                                    if(timer() >= 4000){
                                        rot(1000,30);

                                        CorreçãoDeGiroReto();

                                        while(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                                            motor(100,100);
                                        }
                                        
                                        break;
                                    }
                                }

                                break;
                            }
                        }
                    }

                    Main2();
                }

                //Não é saida...
                if(timer() >= 2000){
                    print(0, "Não é saida");
                    ledRGB(200, 40, 90);
                    
                    motor(-300,-300);
                    delay(300);

                    rot(1000,-80);
                    CorreçãoDeGiroReto();

                    motor(300,300);
                    delay(1500);

                    print(0, " ");
                    goto VoltarVerificaçãoDeSaida;
                }

                //Não é saida, é entrada
                if((corB(2) > corR(2)) && (corB(2) > corG(2)) || (corB(4) > corR(4)) && (corB(4) > corG(4))){
                    ledRGB(100, 100, 100);
                    print(0, "Não é saida");
                    
                    motor(-300,-300);
                    delay(300);

                    rot(1000,-80);
                    CorreçãoDeGiroReto();

                    motor(300,300);
                    delay(1500);

                    print(0, " ");
                    goto VoltarVerificaçãoDeSaida;
                }
            }
        }
    
        //verif parede
        if(sonar(0) < 20 && sonar(2) < 5){     
            print(1, "Parede");
            
            rot(1000,-80);
            CorreçãoDeGiroReto();

            print(1, " ");
            
        }  

        //linha cinza = Entrada
        if((corB(2) > corR(2)) && (corB(2) > corG(2)) || (corB(4) > corR(4)) && (corB(4) > corG(4))){
            ledRGB(100, 100, 100);
            print(1, "Entrada");
            
            rot(1000,-80);
            CorreçãoDeGiroReto();

            motor(300,300);
            delay(1500);

            CorreçãoDeGiroReto();
            print(1, " ");
            
        }
    
        //area de resgate
        if((sonar(0) - sonar(2) >= 20) && sonar(0) < 120 && cor(6) == "PRETO"){
            print(1, "Area de Resgate");
            rot(1000,-45);

            motor(300,300);
            delay(2000);

            rot(1000,-45);
            CorreçãoDeGiroReto();
            print(1, " ");
            
        }

        //Saida!!!
        if(cor(3) == "VERDE" && sonar(0) > 400){
            led("VERDE");
            print(0, "Saida Encontrada");
            
            while(true){
                motor(300,300);

                if(cor(1) == "PRETO" || cor(2) == "PRETO" || cor(3) == "PRETO" || cor(4) == "PRETO" || cor(5) == "PRETO"){
                    break;
                }

                if(sonar(1) > 9000){
                    motor(-300,-300);
                    delay(200);
                    motor(0,0);

                    break;
                }
            }

            led("ciano");
            if(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){

                rstTimer();
                while(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                    motor(-1000,1000);

                    if(timer() >= 2000){
                        rstTimer();

                        while(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                            motor(1000,-1000);
                            
                            if(timer() >= 4000){
                                rot(1000,30);

                                CorreçãoDeGiroReto();

                                while(cor(1) != "PRETO" && cor(2) != "PRETO" && cor(3) != "PRETO" && cor(4) != "PRETO" && cor(5) != "PRETO"){
                                    motor(100,100);
                                }
                                
                                break;
                            }
                        }

                        break;
                    }
                }
            }
            
            Main2();
        }
    }
}

//* Funções Secundárias para resgate
void InitBolinhaNoLado(){
    
    distanciaAtéVitima = sonar(0);
    distanciaAtéBolinha = sonar(1);
    //print(0,distanciaAtéBolinha.ToString());
    led("verde");

    //impulso
    if(resgateDeRe){
    
        motor(-300,-300);//?velocidade ajustável (impulso)
        delay(150);
        
    }
    else{
        //impulso 
        motor(300,300); //?velocidade ajustável (impulso)
        delay(100);
    }
    
    //virar e ficar de frente com a bolinha
    rot(1000,70);
    CorreçãoDeGiroReto();

    if(cor(1) == "VERDE" || cor(2) == "VERDE" || cor(3) == "VERDE" || cor(4) == "VERDE" || cor(5) == "VERDE"){
        rot(1000,-70);
        CorreçãoDeGiroReto(); 

        flagAlarmeFalso = true;

    }else{
        anguloCarrinho = compass();

        //ir para trás
        motor(-300,-300);//?velocidade ajustável (ir para trás)
        delay(200);

        //ir até a bolinha
        tempoDeIda = distanciaAtéBolinha/0.0555;
        //print(1,tempoDeIda.ToString());

        rstTimer();
        while(timer() < tempoDeIda){
            motor(300,300);

            if((anguloCarrinho - compass() >= 5) || (anguloCarrinho - compass() <= -5)){
                CorreçãoDeGiroReto();
            }

            if(sonar(0) - sonar(2) >= 16 && sonar(2) < 15){
                break;
            } 
        }
        
        flagAlarmeFalso = false;

        motor(0,0);
    }
    
    
}

void PegarBolinha(){
    motor(0,0);

    //*pegar bolinha
    if(ClassificarBolinha()){
        voltar3:

        //dar espaço para garra
        rstTimer();
        while(sonar(2) <= 25){
            motor(-300,-300); //?velocidade ajustável (dar espaço para garra)
            
            if(timer() >= 1000){
                //empurra
                motor(100,100);
                delay(1200);
                
                //pegar
                goto voltar3;
            }
        }
        motor(0,0);

        //descer a garra 
        Descer();

        //pegar a bolinha
        rstTimer();
        while(!bc.HasVictim()){
            motor(1000,1000);  //?velocidade ajustável (pegar a bolinha)

            if(timer() >= 2500){
                break;
            }
        }

        //impulso
        motor(300,300); //?velocidade ajustável (impulso)
        delay(100); 

        //subir a garra
        Subir(88, 150);
        motor(0,0);
    }
    
    //!não pegar bolinha
    else{
        distanciaAtéVitimaMorta = distanciaAtéVitima;
        //print(2, distanciaAtéVitimaMorta.ToString());
        flagPegarVitima = false;

        //Voltar para Parede
        rstTimer();
        while(!bc.touch(0)){
            motor(-300,-300);

            if(timer() >= 10000){
                break;
            }
        }

        rot(1000, -80);
        CorreçãoDeGiroReto();

        if(resgateDeRe){
            motor(-300,-300);
            delay(300);
           
        }
        else{
            motor(300,300);
            delay(300);
          
        }

        motor(0,0); 
    }  
}

bool ClassificarBolinha(){    

    //se aproxima da vitima
    while(sonar(2) > 7){
        motor(100,100);
    }

    motor(0,0);
    delay(200);
    
    //verifica vitimas vivas com temperatura alta
    if(cor(6) == "BRANCO" && bc.Heat() > 37){
        //verificar se esta viva
        /*rstTimer();
        while(true){
            motor(-1000,1000);

            //identificou uma vitima morta
            if(cor(6) == "PRETO"){
                CorreçãoDeGiroReto();
                goto VerifDenovo;
            }

            if(timer() >= 1000){
                
                //verificar se esta viva
                rstTimer();
                while(true){
                    motor(1000,-1000);

                    //identificou uma vitima morta
                    if(cor(6) == "PRETO"){
                        CorreçãoDeGiroReto();
                        goto VerifDenovo;
                    }

                    //esta viva
                    if(timer() >= 2000){
                        CorreçãoDeGiroReto();
                        break;
                    }
                }

                break;
            }
        }*/

        numeroBolinhasBrancas++;

        print(2, "B.Brancas:" + "  " + numeroBolinhasBrancas + "  " + "B.Pretas:" + "  " + numeroBolinhasPretas);
        return true;
        
    }
    
    //verifica vitimas mortas com temperatura baixa
    if(cor(6) == "PRETO" && bc.Heat() < 27){
        
        numeroBolinhasPretas++;
        
        print(2, "B.Brancas:" + "  " + numeroBolinhasBrancas + "  " + "B.Pretas:" + "  " + numeroBolinhasPretas);
        
        if(numeroBolinhasBrancas >= 2){
            return true;
        }
        else if(numeroBolinhasPretas >= 2 && numeroBolinhasBrancas >= 2){
            return true;
        }
        else{
            return false;
        }

    }
    
    else{
        return false;
        
    }
    
}

void EntregarKit(){
    motor(0,0);

    rot(1000,-45);

    while(bc.HasRescueKit()){
        Descer();
    }
    
    Subir();
    rot(1000,45);

    CorreçãoDeGiroReto();

}

void EntregarBolinha(){
    led("verde");
    motor(0,0);

    //!dispensar a bolinha

    rot(1000,-45);
    Descer();

    while(bc.HasVictim()){
        
        motor(-100,100);//WIP
        bc.turnActuatorDown(200);
        delay(300);
        bc.TurnActuatorUp(200);
        delay(300);
    }
    
    //espera a bolinha cair
    bc.TurnActuatorUp(200);

    numeroBolinhas++;
    print(1, "Faltam: " + (3 - numeroBolinhas).ToString()+ " bolinhas");

    if(numeroBolinhas == 2){
        flagPegarVitima = true;
    }
    
    Subir();
    
    rot(1000,30);
    CorreçãoDeGiroReto();
}

void CorreçãoDeGiroReto(){
    while(true){
                        
        ledRGB(200, 70, 115);
        
        //Sul
        if(compass() > 135 && compass() < 225){
            while(compass() > 180 ){
                motor(1000,-1000);
            }
            while(compass() < 180 ){
                motor(-1000,1000);
            }
            break;
        }
        //Leste
        if(compass() > 45 && compass() < 135){
            while(compass() > 90 ){
                motor(1000,-1000);
            }
            while(compass() < 90 ){
                motor(-1000,1000);
            }
            break;
        }
        //Norte
        if(compass() < 45 || compass() > 315){
            while(compass() > 1 && compass() < 45){
                motor(1000,-1000);
            }
            while(compass() < 360 && compass() > 315){
                motor(-1000,1000);
            }
            break;
        }
        //Oeste
        if(compass() > 225 && compass() < 315){
            while(compass() > 270 ){
                motor(1000,-1000);
            }
            while(compass() < 270 ){
                motor(-1000,1000);
            }
            break;
        }
    }
    ledOff();
    motor(0,0);
}