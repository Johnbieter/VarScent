#define motorPin 2

char myCol[20];

void setup() {
  // put your setup code here, to run once:
  pinMode(motorPin, OUTPUT);
  Serial.begin(9600);
 

}

void loop() {
  // put your main code here, to run repeatedly:
  
   if(Serial.available() > 0)
{
    char input = Serial.peek();
    
   int ltr = (int)input; //Serial.peak;
   if(ltr == 1)
   {
    digitalWrite(motorPin,HIGH);    
   }
   else if(ltr == 0)
   {
    digitalWrite(motorPin, LOW);
    }
}
}
