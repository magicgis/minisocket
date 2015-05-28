int ledPin = 13;

void setup() {
  Serial.begin(9600);
  pinMode(ledPin, OUTPUT);
}

void loop() {
  byte cmd;
  if(Serial.available()) {
    cmd = Serial.read();
    if(cmd == '0') {
      digitalWrite(ledPin, LOW);
    }
    else if(cmd == '1') {
      digitalWrite(ledPin, HIGH);
    }
  }
}
