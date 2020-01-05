@ECHO OFF

del .\bin\release\netcoreapp2.2\publish\appsettings.json
del .\bin\release\netcoreapp2.2\publish\appsettings.notebruno.json
del .\bin\release\netcoreapp2.2\publish\appsettings.zenBookFlip.json
ren .\bin\release\netcoreapp2.2\publish\appsettings.production.json appsettings.json 

docker build -t clearspam.web ./bin/release/netcoreapp2.2/publish

docker tag clearspam.web registry.heroku.com/clearspam/web

docker push registry.heroku.com/clearspam/web

heroku container:release web -a clearspam

PAUSE