# #!/bin/bash

# set -e 

# git stash
# CURRENT_BRANCH=$(git rev-parse --abbrev-ref HEAD)
# npm run build
# git branch -D gh-pages || true
# git checkout -f --orphan gh-pages
# rm .gitignore
# git add dist/*
# git mv dist/* .
# git clean -f
# git commit -am 'Website'
# git checkout $CURRENT_BRANCH
# git stash pop 

rmdir /s dist
npm run build
copy CNAME dist\CNAME
cd dist
git init
git config core.autocrlf true
git config core.safecrlf false
git add .
git commit -m "Website"
git branch gh-pages
git checkout gh-pages
git remote add origin https://github.com/theolivenbaum/tinder4cats.git
git push --set-upstream origin gh-pages --force
cd ..
rmdir /s dist