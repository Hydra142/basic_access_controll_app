SELECT Password
FROM PasswordHistory
WHERE UserId = @UserId
ORDER BY Id DESC
LIMIT 3