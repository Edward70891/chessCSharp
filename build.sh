#!/bin/bash

mcs -t:library chess.cs
mcs -r:chess.dll main.cs
mv chess.dll build
mv main.exe build
