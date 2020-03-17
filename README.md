# BiDirectionalIce_PythonCSharp
Demonstarting a bi-directional remote process communications between C# and Python using ZeroC Ice.

![screenshot](https://i.imgur.com/9Hu7WCX.png)

The bi-directionality is implemented by making both sides clients and servers.

To run the python side:
```
cd <repository>
cd python
python main.py
```

To run the C# side:
```
cd <repository>
cd csharp\csharp\csharp\bin\debug
csharp.exe
```

To re-compile the slice (.ice file) after you made changes to it:
```
cd <repository>
compile_ice.bat
```
