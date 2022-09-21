app.exe: long_double_time.cpp long_double_time.h
	g++ -o app.exe long_double_time.cpp -DFPUX87 -m80387 -DASMTEST -mfpmath=387 

run: app.exe
	./app.exe


