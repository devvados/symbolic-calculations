grammar Calculator;

/// Parser

prog: expr+ ;

expr : VARIABLE												#Identity
	 | '-' expr												#Minus
     | expr '^' expr										#Pow
     | expr op=('*'|'/') expr								#MulDiv
     | expr op=('+'|'-') expr								#AddSub
	 | expr expr											#MultNoSign
     | VAL													#Val
     | fun=('ln' | 'exp' | 'sqrt' | 
			'sin' | 'cos' | 'tan' | 'cot' |
			'sec' | 'csc' |
			'arcsin' | 'arccos' | 'arctan' | 'arccot' |
			'arcsec' | 'arccsc' | 
			'sh' | 'ch' | 'th' | 'cth' ) '(' expr ')'		#Function									
     | '(' expr ')'											#Parens
     ;

/// Lexer

VAL : [0-9]+ ( '.' [0-9]+ )?;
VARIABLE: ('x' | 'y' | 'z');
//Operations
POW : '^';
MUL : '*';
DIV : '/';
ADD : '+';
SUB : '-';
//Elementary
SQRT: 'sqrt';
LN : 'ln';
EXP: 'exp';
//Trigonometric
SIN : 'sin';
COS : 'cos';
SEC: 'sec';
CSC: 'csc';
TAN: 'tan';
COT: 'cot';
//Inverse Trigonometric
ARCSIN: 'arcsin';
ARCCOS: 'arccos';
ARCTAN: 'arctan';
ARCCOT: 'arccot';
ARCSEC: 'arcsec';
ARCCSC: 'arccsc';
//Hyperbolic
SH: 'sh';
CH: 'ch';
TH: 'th';
CTH: 'cth';

WS  : (' '|'\r'|'\n') -> channel(HIDDEN);