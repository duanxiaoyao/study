import Qs from "qs";
import axios from "axios";

const Request = {
  url: (url) => {
    return "http://localhost:1111/api/" + url
  },
  post: (url, data, callback) => {
    axios({
        method: 'get',
        url: Request.url(url),
        params: Qs.stringify({
          data
        }),
        headers: {
          "Content-type": "application/json"
        }
      }).then(function (respose) {
        callback(respose.data)
      })
      .then(function () {})
      .catch(function (respose) {})
  },
  post: (url, data, callback) => {
    axios({
        method: 'post',
        url: Request.url(url),
        params: data,
        headers: {
          "Content-type": "application/json-patch+json"
        }
      }).then(function (respose) {
        callback(respose.data)
      })
      .then(function () {})
      .catch(function (respose) {})
  }
}
export default Request
