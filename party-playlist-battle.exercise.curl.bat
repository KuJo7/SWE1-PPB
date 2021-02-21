@echo off

REM --------------------------------------------------
REM Party Playlist Battle
REM --------------------------------------------------
title Party Playlist Battle - Joel KUDIYIRICKAL
echo CURL Testing for Party Playlist Battle
echo.

REM --------------------------------------------------
echo 1) Create Users (Registration)
REM Create User
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.
echo.
echo should fail:
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:10001/users --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo. 
echo.

REM --------------------------------------------------
echo 2) Login Users
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"daniel\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"altenhof\", \"Password\":\"markus\"}"
echo.
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"admin\",    \"Password\":\"istrator\"}"
echo.
echo.
echo should fail:
curl -X POST http://localhost:10001/sessions --header "Content-Type: application/json" -d "{\"Username\":\"kienboec\", \"Password\":\"different\"}"
echo.
echo.

REM --------------------------------------------------
echo 3) edit user data
echo.
curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Basic altenhof-ppbToken"
echo.
curl -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"Name\": \"Kienboeck\",  \"Bio\": \"me playin...\", \"Image\": \":-)\"}"
echo.
curl -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"Name\": \"Altenhofer\", \"Bio\": \"live long and prosper...\",  \"Image\": \":-D\"}"
echo.
curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Basic altenhof-ppbToken"
echo.
echo.
echo should fail:
curl -X GET http://localhost:10001/users/altenhof --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X GET http://localhost:10001/users/kienboec --header "Authorization: Basic altenhof-ppbToken"
echo.
curl -X PUT http://localhost:10001/users/kienboec --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"Name\": \"Hoax\",  \"Bio\": \"asdf...\", \"Image\": \":-)\"}"
echo.
curl -X PUT http://localhost:10001/users/altenhof --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"Name\": \"Hoax\", \"Bio\": \"asdf...\",  \"Image\": \":-D\"}"
echo.
curl -X GET http://localhost:10001/users/someGuy  --header "Authorization: Basic kienboec-ppbToken"
echo.
echo.

REM --------------------------------------------------
echo 4) stats
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-ppbToken"
echo.
echo.

REM --------------------------------------------------
echo 5) scoreboard
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X GET http://localhost:10001/score --header "Authorization: Basic altenhof-ppbToken"
echo.
echo.

REM --------------------------------------------------
echo 6) get library
curl -X GET http://localhost:10001/lib --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X GET http://localhost:10001/lib --header "Authorization: Basic altenhof-ppbToken"
echo.
echo.

REM --------------------------------------------------
echo 7) manipulate (add)
curl -X POST http://localhost:10001/lib --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"Name\": \"Best_song_ever\", \"Url\": \"https://www.youtube.com/watch?v=dQw4w9WgXcQ\", \"Rating\": 5, \"Genre\": \"Pop\"}"
echo.
curl -X POST http://localhost:10001/lib --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"Name\": \"Good_Mood_Song\", \"Url\": \"https://youtu.be/H9cmPE88a_0\", \"Rating\": 4, \"Genre\": \"Pop\", \"Title\": \"Duck Tales Intro\", \"Length\": \"2:52\", \"Album\": \"Theme Songs\"}"
echo.
curl -X POST http://localhost:10001/lib --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"Name\": \"Super_Mario_song\", \"Url\": \"https://www.youtube.com/watch?v=wyoNnMO3zFk\", \"Rating\": 4, \"Genre\": \"Game Music\", \"Title\": \"Super Mario\"}"
echo.
echo.

REM --------------------------------------------------
echo 8) get library
curl -X GET http://localhost:10001/lib --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X GET http://localhost:10001/lib --header "Authorization: Basic altenhof-ppbToken"
echo.
echo.

REM --------------------------------------------------
echo 9) manipulate (remove)
curl -X DELETE http://localhost:10001/lib/Best_song_ever --header "Authorization: Basic altenhof-ppbToken"
echo.
echo.
echo should fail:
curl -X DELETE http://localhost:10001/lib/Best_song_ever --header "Authorization: Basic altenhof-ppbToken"
echo.
curl -X DELETE http://localhost:10001/lib/unknown --header "Authorization: Basic altenhof-ppbToken"
echo.
curl -X DELETE http://localhost:10001/lib/Super_Mario_song --header "Authorization: Basic kienboec-ppbToken"
echo.
echo.

REM --------------------------------------------------
echo 10) get playlist
curl -X GET http://localhost:10001/playlist
echo.
echo.

REM --------------------------------------------------
echo 11) add song to playlist
curl -X POST http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"Name\": \"Super_Mario_song\"}"
echo.
curl -X POST http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"Name\": \"Super_Mario_song\"}"
echo.
curl -X POST http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"Name\": \"Super_Mario_song\"}"
echo.
echo.
curl -X POST http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"Name\": \"Good_Mood_Song\"}"
echo.
curl -X POST http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"Name\": \"Good_Mood_Song\"}"
echo.
echo.

echo should fail:
curl -X POST http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"Name\": \"Super_Mario_song\"}"
echo.
curl -X POST http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"Name\": \"unknown\"}"
echo.
echo.

REM --------------------------------------------------
echo 12) get playlist
curl -X GET http://localhost:10001/playlist
echo.
echo.

REM --------------------------------------------------
echo 13) reorder (should fail, not admin)
curl -X PUT http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"FromPosition\": 1, \"ToPosition\": 4}"
echo.
echo.

REM --------------------------------------------------
echo 14) set actions (kienboec)
curl -X GET http://localhost:10001/actions --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X PUT http://localhost:10001/actions --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"actions\": \"RRRRR\"}"
echo.
curl -X GET http://localhost:10001/actions --header "Authorization: Basic kienboec-ppbToken"
echo.
echo.
echo should fail:
curl -X PUT http://localhost:10001/actions --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"actions\": \"S\"}"
echo.
curl -X PUT http://localhost:10001/actions --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"actions\": \"\"}"
echo.
curl -X PUT http://localhost:10001/actions --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"actions\": \"AAAAA\"}"
echo.
echo.
echo check, that failed:
curl -X GET http://localhost:10001/actions --header "Authorization: Basic kienboec-ppbToken"
echo.
echo.
echo set actions (altenhof):
curl -X GET http://localhost:10001/actions --header "Authorization: Basic altenhof-ppbToken"
echo.
curl -X PUT http://localhost:10001/actions --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"actions\": \"SSSSS\"}"
echo.
curl -X GET http://localhost:10001/actions --header "Authorization: Basic altenhof-ppbToken"
echo.
echo.

REM --------------------------------------------------
echo 15) battle (kienboec starts the 15 seconds tournament)
start /b "kienboec battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic kienboec-ppbToken"
echo.
start /b "altenhof battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic altenhof-ppbToken"
ping localhost -n 20 >NUL 2>NUL
echo.
echo.

REM --------------------------------------------------
echo 16) stats
curl -X GET http://localhost:10001/stats --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X GET http://localhost:10001/stats --header "Authorization: Basic altenhof-ppbToken"
echo.
echo.

REM --------------------------------------------------
echo 17) scoreboard
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X GET http://localhost:10001/score --header "Authorization: Basic altenhof-ppbToken"
echo.
echo.

REM --------------------------------------------------
echo 18) reorder (admin)
curl -X PUT http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"FromPosition\": 1, \"ToPosition\": 4}"
echo.
curl -X GET http://localhost:10001/playlist
echo.
echo.

REM --------------------------------------------------
echo 19) set actions to simulate a draw
curl -X PUT http://localhost:10001/actions --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"actions\": \"RRRRR\"}"
echo.
echo.

REM --------------------------------------------------
echo 20) battle (no admin)
start /b "kienboec battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic kienboec-ppbToken"
echo.
start /b "altenhof battle" curl -X POST http://localhost:10001/battles --header "Authorization: Basic altenhof-ppbToken"
ping localhost -n 20 >NUL 2>NUL
echo.
echo.

REM --------------------------------------------------
echo 21) reorder (old Admin)
curl -X PUT http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic kienboec-ppbToken" -d "{\"FromPosition\": 1, \"ToPosition\": 3}"
echo.
echo.
echo should fail:
curl -X PUT http://localhost:10001/playlist --header "Content-Type: application/json" --header "Authorization: Basic altenhof-ppbToken" -d "{\"FromPosition\": 1, \"ToPosition\": 3}"
echo.
echo.

REM --------------------------------------------------
echo 22) scoreboard
curl -X GET http://localhost:10001/score --header "Authorization: Basic kienboec-ppbToken"
echo.
curl -X GET http://localhost:10001/score --header "Authorization: Basic altenhof-ppbToken"
echo.
echo.

echo.
echo.
echo.

pause
@echo on
