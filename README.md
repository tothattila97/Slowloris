# Slowloris

C# version of the known Slowloris tool, which is a type of DoS attack. This approach allows the attacker to make a web server unavailable meanwhile using low bandwith.

## Available connectors

``` 
-h: use in order to determine the host  (default value: 127.0.0.1)
-p: use to set the port of the host (default value: 80)
-c: use to set the number of connections (default value: 200)
```
## Basic usage

If you would like to use the tool, you only need to run the Program.exe. You can skip any of the connectors, in that case
the application will run with the default values.

``` 
1. git clone https://github.com/tothattila97/Slowloris.git
2. cd Slowloris\SlowLoris
3. Program.exe -h <HOST_ADDRESS> -p <HOST_PORT> -c <NUMBER_OF_SOCKETS>
``` 
