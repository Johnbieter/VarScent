
#define Red2 2 
#define Red1 3
#define Yellow2 4
#define Yellow1 5
#define Green2 6
#define Green1 7
#define Blue 8 

void setup() {
  pinMode(Red2, OUTPUT); // Declare the LED as an output
  pinMode(Red1, OUTPUT);
  pinMode(Yellow2, OUTPUT);
  pinMode(Yellow1, OUTPUT);
  pinMode(Green2, OUTPUT);
  pinMode(Green1, OUTPUT);
  pinMode(Blue, OUTPUT);
}

// the loop function runs over and over again forever
void loop() {
  digitalWrite(Red2, HIGH);
  delay(1000);                     
  digitalWrite(Red2, LOW);  
  delay(1000);     
  digitalWrite(Red1, HIGH);
  delay(1000);                     
  digitalWrite(Red1, LOW);  
  delay(1000);  
  digitalWrite(Yellow2, HIGH);
  delay(1000);                     
  digitalWrite(Yellow2, LOW);  
  delay(1000);  
  digitalWrite(Yellow1, HIGH);
  delay(1000);                     
  digitalWrite(Yellow1, LOW);  
  delay(1000);  
  digitalWrite(Green2, HIGH);
  delay(1000);                     
  digitalWrite(Green2, LOW);  
  delay(1000);   
  digitalWrite(Green1, HIGH);
  delay(1000);                     
  digitalWrite(Green1, LOW);  
  delay(1000);          
  digitalWrite(Blue, HIGH);      
  delay(1000);                     
  digitalWrite(Blue, LOW);  
  delay(1000);  
}
