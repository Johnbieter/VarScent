
int incomingByte = 0; // for incoming serial data
int RED = 13;
int GREEN = 12;

void setup() {
  Serial.begin(9600); // opens serial port, sets data rate to 9600 bps
  pinMode(RED, OUTPUT);
  pinMode(GREEN, OUTPUT);
}


void loop() {
  // send data only when you receive data:
  if (Serial.available() > 0) {
    // read the incoming byte:
    incomingByte = Serial.read();

//The first byte of incoming serial data available 
//(or -1 if no data is available). Data type: int.
   if(incomingByte > 49.5){
    Serial.println(incomingByte);//, DEC);
    analogWrite(RED, 255);
    //digitalWrite(GREEN, LOW);
   // delay(300);
   }
   else
   {
    //Serial.println("NULL");//, DEC);
    //Serial.println(incomingByte);
    //digitalWrite(GREEN, HIGH);
    analogWrite(RED, 200);
   // delay(300);
   }
   
    
    
  }
}
