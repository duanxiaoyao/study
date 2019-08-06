/* 是否手机号码*/
export function validatePhone(rule, value, callback) {
    const reg = /^((0\d{2,3}-\d{7,8})|(1[34578]\d{9}))$/;
    if (value == '' || value == undefined || value == null) {
      callback(new Error('请输入手机号'));
    } else {
      if ((!reg.test(value)) && value != '') {
        callback(new Error('请输入正确的手机号'));
      } else {
        callback();
      }
    }
  }
/* 是否身份证号码*/
export function validateIdNo(rule, value,callback) {
    const reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    if(value==''||value==undefined||value==null){
      callback();
    }else {
      if ((!reg.test(value)) && value != '') {
        callback(new Error('请输入正确的身份证号码'));
      } else {
        callback();
      }
    }
  }
  /*验证账号是否英文数字以及下划线*/
  export function validateIdName(rule, value, callback) {
    const reg = /^[_a-zA-Z0-9]+$/;
    if (value.length < 6) {
      callback(new Error("账号不得小于6位数"));
    } 
    else if (!reg.test(value)) {
        callback(new Error("账号为英文字母，数字以或下划线组成"));
      } else {
        callback();
      }
  }

  /*验证密码包含字母和数字*/
  export function validateIdPassWord(rule, value, callback) {
    const reg = /^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,16}$/;
    if (value.length < 6) {
      callback(new Error("密码不得小于6位数"));
    }
    else if (!reg.test(value)) {
        callback(new Error("密码须同时包含字母和数字"));
      } else {
        callback();
      }
  }

