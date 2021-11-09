int incomingByte = 0; // for incoming serial data
int relay = 7;

void setup() {
  Serial.begin(9600); // opens serial port, sets data rate to 9600 bps
  pinMode(relay, OUTPUT);
  
}


void loop() {
  //digitalWrite(relay,HIGH);
  
  if (Serial.available() > 0) {
    // read the incoming byte:
    incomingByte = Serial.read();


   if(incomingByte > 49.5){
    
    digitalWrite(relay, HIGH);
   
  
   }
   else
   {

    digitalWrite(relay, LOW);

   }
   
    
    
  }
}
