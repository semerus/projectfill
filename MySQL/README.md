# Running command
$ mysql -u[USER] -p[Password] [DATABASE] < [FILE\_NAME].mysql

USER/PASSWORD : user, password used to login to mysql
DATABASE      : name of the database where mock data is created

# File running order
1. creation.sql
2. mock\_user.sql
3. mock\_category.sql
4. mock\_game.sql
5. mock\_playdata.sql
6. mock\_gamedata.sql

# load_file(), secure_file_priv 관련 trouble shooting
 * my.ini/my.cnf 파일을 수정 (secure_file_priv)
 * D: 드라이브에 있는 폴더로는 불가능 했음
 * 앞뒤에 "" 로 감싸주기
 * MySQL 재시작 방법
  * 재시작 방법(Windows): win+R -> services.msc -> mysql 재시작
  * 관리자 권한 cmd 에서
    * net stop mysql80
    * net start mysql80

http://coffeenix.net/board_view.php?bd_code=1707
