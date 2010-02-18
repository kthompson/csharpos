GOALS
===============================

1. Create a CIL to x86 compiler that could later be used to create a csharp/cil based OS.
2. Create a new CLR written in .Net that can be converted to x86 via the compiler
3. Create a CIL based Operating System

DEVELOPMENT
===============================
In order to run the tests you must first download and install a few dependecies:

1. MinGW (http://www.mingw.org/) and add its install path to your PATH environment variable
2. Gallio (http://www.gallio.org/) for unit tests
3. Begin hacking away. 

Other tools that may be helpful:
 
* IDAPro (http://www.hex-rays.com/idapro/)

BRANCHES
===============================
* master
	> main development branch.
		
* pu
	> commits to be reviewed and included into master. 
	> Subject to rebasing.
		
* cecil
	> Import of Mono.Cecil svn repository
		
* decompiler
	> Import of Cecil.Decompiler svn repository

LICENSE
===============================

	Copyright (c) 2010, Kevin Thompson
	All rights reserved.
	
	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:
		* Redistributions of source code must retain the above copyright
    	  notice, this list of conditions and the following disclaimer.
    	* Redistributions in binary form must reproduce the above copyright
    	  notice, this list of conditions and the following disclaimer in the
    	  documentation and/or other materials provided with the distribution.
    	* Neither the name of the Kevin Thompson nor the
    	  names of its contributors may be used to endorse or promote products
    	  derived from this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
	ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
	WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	DISCLAIMED. IN NO EVENT SHALL KEVIN THOMPSON BE LIABLE FOR ANY
	DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
	(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
	LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.