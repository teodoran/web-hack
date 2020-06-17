3. Cross Site Scripting (XSS)
=============================
_Inject some JS to gain information from other user._

The Fault: Example inject JS to own notes
-----------------------------------------

This does not work. Probably due to browser built-in XXS-detection.
```html
<script>alert("XSS");</script>
```

This does work. Should reference OWASP XXS Filter Evasion Cheat Sheet.
```html
<svg/onload=alert('XSS')>
```

References:
* [OWASP: Cross Site Scripting (XSS)](https://owasp.org/www-project-top-ten/OWASP_Top_Ten_2017/Top_10-2017_A7-Cross-Site_Scripting_(XSS))
* [OWASP: XSS Filter Evasion Cheat Sheet](https://owasp.org/www-community/xss-filter-evasion-cheatsheet)

The Fix: Something with innerText vs. inner HTML
------------------------------------------------

Change `content.innerHTML = note.content;` to `content.innerText = note.content;`
Think about what's valid when validating in frontend and API. Strong validation helps.

The Flag
--------

