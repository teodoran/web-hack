2 Broken Access Control
=======================
_In this hack we'll find notes for another user. Authenticated users can access other users resources._

The Fault: Retrieve notes from other user by changing URL
---------------------------------------------------------

Go over GET /notes/id until we find a note not made by current user.

References:
* [OWASP: Broken Access Control](https://owasp.org/www-project-top-ten/OWASP_Top_Ten_2017/Top_10-2017_A5-Broken_Access_Control)

The Fix: Check user ID as part of auth
--------------------------------------

The Flag
--------
