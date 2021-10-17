
//!Functions return => void
    //print(n,msg)
    void print(int n, string msg)
        {bc.printLCD(n, msg);}

    //delay(n)
    void delay(int n)
        {bc.wait(n);}

    //motor(left,right)
    void motor(int left, int right)
        {bc.onTF(left, right);}

    //rot(speed,angle)
    void rot(int speed, float angle)
        {bc.onTFRot(speed, angle);}

//!Functions return => value
    //cor(sensor)
    string cor(int sensor) 
        {return bc.returnColor(sensor);}

    //luz(sensor)
    float luz(int sensor) 
        {return bc.lightness(sensor - 1);}

    //corB(sensor)
    float corB(int sensor) 
        {return bc.returnBlue(sensor);}

    //corG(sensor)
    float corG(int sensor) 
        {return bc.returnGreen(sensor);}

    //corR(sensor)
    float corR(int sensor) 
        {return bc.returnRed(sensor);}

//! General Functions

void clawDown(int speed, int angle)
{bc.actuatorSpeed(speed); bc.actuatorDown(angle);}

void clawUp(int speed, int b)
{bc.actuatorSpeed(speed); bc.actuatorUp(angle);}



void Main(){
    while (true)
    {	
        motor(100,100);
        delay(1000);
        motor(-100,-100);
        delay(1000);
        motor(-100,100);
        delay(1000);
        motor(100,-100);
        delay(1000);

    }
}
