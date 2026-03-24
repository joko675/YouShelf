# YouShelf API Docs
## Auth

### POST /auth/register
Registers new user with username and password, returns newly created user id
#### Example request
```json
{
  "username":"test",
  "password":"test"
}
```
#### Example response
```json
{
  "success": true,
  "userId": 2,
  "errorMessage": null
}
```
### POST /auth/login
Authenticate user, return jwt token
#### Example request
```json
{
  "username":"test",
  "password":"test"
}
```
#### Example response
```json 
{
  "success": true,
  "token": "testToken",
  "errorMessage": null
}
```

## Book
### POST /book
Add new book. Returns newly created book info. Require authorization header
#### Header 
```http
Authorization: Bearer <jwt token>
```
#### Example request
```json
{
  "title": "Harry Potter",
  "author": "J.K. Rowling",
  "description": "TestDescription",
  "releaseDate": "2001-01-01",
  "imageUrl": "TestUrl",
  "review": "TestReview"
}
```
#### Example response
```json
{
  "bookId": 14,
  "title": "Harry Potter",
  "author": "J.K. Rowling",
  "description": "TestDescription",
  "releaseDate": "2001-01-01",
  "imageUrl": "TestUrl",
  "review": "TestReview",
  "userId": 1
}
```
### GET /book
Get list of books owned by currently logged user. Require authorization header
#### Header 
```http
Authorization: Bearer <jwt token>
```
#### Parameters
| Parameter | Type   | Required | Default | Description              |
|-----------|--------|----------|---------|--------------------------|
| title     | string | false    | null    | Filters books by title   |
| author	| string | false	| null	  | Filters books by author	 |
| limit		| int	 | false	| 10	  | Maximum number of results|
| offset	| int	 | false	| 0		  | How many results to skip |
#### Example request
`/book?title=Harry+Potter&author=J.K.+Rowling&limit=10&offset=0`
#### Example response
```json
[
  {
    "bookId": 1,
    "title": "Harry Potter",
    "author": "J.K. Rowling",
    "description": "TestDescription",
    "releaseDate": "2001-01-01",
    "imageUrl": "TestUrl",
    "review": "TestReview",
    "userId": 1
  }
]
```
