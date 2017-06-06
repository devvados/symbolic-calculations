grammar Calculator;

/// Parser

prog: expr+ ;

expr : '-' expr						  # Minus
     | expr '^' expr                  # Pow
     | expr op=('*'|'/') expr         # MulDiv
     | expr op=('+'|'-') expr         # AddSub
	 | expr expr					  #MultNoSign
     | VAL                            # Val
     | fun=('sin'|'cos'|'sqrt' | 'ln' | 'exp' | 
			'arcsin' | 'arccos' | 'arctan' | 'arccot' |
			'sh' | 'ch' | 'th' | 'cth' | 'sec' | 'cosec' ) '(' expr ')' # Function
	 | 'x'							  # Identity
     | '(' expr ')'                   # Parens
     ;

/// Lexer

VAL : [0-9]+ ( '.' [0-9]+ )?;
POW : '^';
MUL : '*';
DIV : '/';
ADD : '+';
SUB : '-';
X: 'x';
SIN : 'sin';
COS : 'cos';
SQRT: 'sqrt';
LN : 'ln';
EXP: 'exp';
ARCSIN: 'arcsin';
ARCCOS: 'arccos';
ARCTAN: 'arctan';
ARCCOT: 'arccot';
SH: 'sh';
CH: 'ch';
TH: 'th';
CTH: 'cth';
SEC: 'sec';
COSEC: 'cosec';
WS  : (' '|'\r'|'\n') -> channel(HIDDEN);