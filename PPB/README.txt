Design:
	Das Projekt ist auf dem Webserver (1. Projekt) aufgebaut. 
	In der Projektmappe PPB befinden sich die Klassen PPB, RequestContext, User, UserCredentials, MultiMediaContent, MultiMediaContentName, PositionChange, Actions, ActionString, DBHandler, BattleHandler, Scoreboard.

	BattleHandler:
		Battle(): Es wird überprüft ob Player1 oder Player2 gewinnt. In der Battle() Methode wird die ActionBattle() Methode aufgerufen. Es wird der winner, loser und das log zurückgegeben.
		ActionBattle(): Es wird überprüft welcher Player die einzelnen Runden gewinnt mit Berücksichtigung der Effectiveness. Falls ein Player eine Runde gewinnt, wird die Username ausgegeben und die BattlePoints erhöht. 
				Im Falle eines Draws wird ein 'X' ausgegeben. 
	DBHandler:
		Register(): Für die Registrierung der User.
		Login(): Für das Einloggen der registrierten User.
		Connect(): Zum Verbinden zur Datenbank
		StoreMultiMediaContent(): Das MultiMediaContent wird in der Datenbank gespeichert.
		StoreIDs: Es werden die IDs des MultiMediaContent und des Users gespeichert.
		DeleteMultiMediaContent(): Das MultiMediaContent wird aus der Datenbank gelöscht.
		GetAllUsers(): Alle registrierten User werden angezeigt.
		GetUser(): Das Profil eines Users wird angezeigt.
		UpdateStats(): Es werden die UserStats in der Datenbank aktualisiert.
		UpdateProfile(): Es werden die Daten vom Profil in der Datenbank aktualisiert.
		ChangeUsername(): Die Username der User kann geändert werden.
		ChangePassword(): Das Passwort der User kann geändert werden
	
	Tables in the database:
		users: Userdaten
		multimediacontent: Songs
		multimediacontentusers: Zwischentabelle für die IDs

	Ich habe sehr früh mit dem Projekt begonnen und habe daher zeitlich keine Probleme gehabt. Daher habe ich das Projekt vollständig umsetzen können.

Unit test design:
	Der Fokus bei den Unit Tests wurden auf die Klassen BattleHandler, DBHandler und PPB gelegt und insgesamt umfasst das Projekt 15 Unit Tests. 
	Ich habe mir die Unit Tests ausgewählt, wo ich das Gefühl hatte dass diese kritisch sind und möglicherweise nicht optimal funktionieren könnte.
	Der Code ist kritisch weil es zb. beim Battle möglicherweise die Effectiveness nicht richtig berücksichtigt wurde oder die ProcessRequest() nicht richtig ausgeführt wird aufgrund des security-tokens und den unterschiedlichen requests.
	Die Unit Tests beweisen dass alle kritischen Bereiche gesichert wurden. 

Time spent:
 	Ich habe ungefähr 20 Stunden für das Projekt investiert. 

Link to git:
	https://github.com/joelkudiyi/SWE1-PPB