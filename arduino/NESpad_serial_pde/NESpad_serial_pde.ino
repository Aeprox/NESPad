#include <NESpad.h>

// put your own strobe/clock/data pin numbers here -- see the pinout in readme.txt
NESpad nintendo = NESpad(2,3,4);

byte state = 0;

void setup() {
  Serial.begin(57600);  
}

void loop() {
  
  state = nintendo.buttons();

  // shows the shifted bits from the joystick
  // buttons are high (1) when up 
  // and low (0) when pressed
  Serial.println(~state, BIN);
  

  delay(50);
}
