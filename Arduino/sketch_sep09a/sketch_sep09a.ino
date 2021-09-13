#define Red 4 
#define Yellow 3
#define Green 2 



// The pin the LED is connected to
void setup() {
  Serial.begin(9600);
  pinMode(Red, OUTPUT); // Declare the LED as an output
  pinMode(Yellow, OUTPUT);
  pinMode(Green, OUTPUT);

  digitalWrite(Green,LOW);
    digitalWrite(Yellow,LOW);
    digitalWrite(Red,LOW);
}

void loop() {

 if(Serial.available() > 0)
{
   char ltr = Serial.read();
   if(ltr == 'A')
   {
    digitalWrite(Green,HIGH);
   }
   else if(ltr == 'B')
   {
    digitalWrite(Yellow,HIGH);
   }
   else if(ltr == 'C')
   {
    digitalWrite(Red, HIGH);
    }
    
    
}
digitalWrite(Green,LOW);
    digitalWrite(Yellow,LOW);
    digitalWrite(Red,LOW);
}
