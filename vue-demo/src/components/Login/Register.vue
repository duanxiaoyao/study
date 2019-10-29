<template>
  <div class="login-wrap">
    <div class="ms-login">
      <div class="ms-title">注册</div>
      <el-form :model="form" class="ms-content" ref="form" :rules="rules" status-icon>
        <el-form-item prop="loginCode">
          <el-input
            v-model="form.loginCode"
            auto-complete="off"
            placeholder="请输入账号"
          ></el-input>
        </el-form-item>
        <el-form-item prop="passWord">
          <el-input
            type="password"
            v-model="form.passWord"
            auto-complete="off"
            placeholder="设置密码"
          ></el-input>
        </el-form-item>
        <el-form-item prop="checkPassword">
          <el-input
            type="password"
            v-model="form.checkPassword"
            auto-complete="off"
            placeholder="确认密码"
          ></el-input>
        </el-form-item>
        <el-form-item prop="phone">
          <el-input
            v-model="form.phone"
            auto-complete="off"
            placeholder="请输入手机号"
            @change="checkPhone"
          ></el-input>
        </el-form-item>
        <el-form-item prop="code">
          <el-input v-model="form.code" auto-complete="off" placeholder=" 请输入验证码" style="width:63%"></el-input>
          <el-button
            @click="getCode()"
            v-if="btnTitle"
            :disabled="disabled"
            style="width:35%"
          >{{btnTitle}}</el-button>
        </el-form-item>
        <div class="login-btn">
          <el-button type="primary" @click="register('form')">立即注册</el-button>
        </div>
      </el-form>
    </div>
  </div>
</template>

<script>
import { validatePhone, validateIdPassWord } from "./validate";
export default {
  /** 组件名称 */
  name: "Register",
  /** 绑定数据 */
  data() {
    /*验证账号是否英文数字以及下划线*/
    var validateIdName = (rule, value, callback) => {
      const reg = /^[_a-zA-Z0-9]+$/;
      if (value.length < 6) {
        console.log(callback);
        callback(new Error("账号不得小于6位数"));
      } else if (!reg.test(value)) {
        callback(new Error("账号为英文字母，数字以或下划线组成"));
      } else if (reg.test(value)) {
        this.isExistName(callback);
      } else {
        callback();
      }
    };
    /** 二次密码检测 */
    var checkPassword = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请再次输入密码"));
      } else if (value !== this.form.passWord) {
        callback(new Error("两次输入密码不一致"));
      } else {
        callback();
      }
    };
    /** 验证码检测 */
    var checkCode = (rule, value, callback) => {
      let code = sessionStorage.getItem("code");
      if (value === "") {
      } else if (value !== code) {
        callback(new Error("验证码错误"));
      } else {
        callback();
      }
    };
    return {
      /** 表单数据 */
      form: {
        loginCode: "",
        passWord: "",
        checkPassword: "",
        phone: "",
        code: ""
      },
      mobile_code: "",
      disabled: true,
      btnTitle: "获取验证码",
      rules: {
        loginCode: [{ validator: validateIdName, trigger: "blur" }],
        passWord: [{ validator: validateIdPassWord, trigger: "blur" }],
        checkPassword: [{ validator: checkPassword, trigger: "blur" }],
        phone: [{ validator: validatePhone, trigger: "blur" }],
        code: [{ validator: checkCode, trigger: "blur" }]
      }
    };
  },
  /** 回车注册事件 */
  created() {
    var _this = this;
    document.onkeydown = function(e) {
      var key = window.event.keyCode;
      if (key == 13) {
        _this.register("form");
      }
    };
  },
  methods: {
    isExistName(callback) {
      this.$request.post("Login/UserExist", this.form, response => {
        if (response.data == true) {
          callback(new Error("账号已存在"));
        } else {
          callback();
        }
      });
    },
    /** 号码正确验证按钮可使用 */
    checkPhone() {
      const reg = /^((0\d{2,3}-\d{7,8})|(1[34578]\d{9}))$/;
      if (reg.test(this.form.phone)) {
        this.disabled = false;
      } else this.disabled = true;
    },
    /** 获得验证码 */
    getCode() {
      window.sessionStorage.removeItem("code");
      this.validateBtn();
      this.$request.post("Login/PhoneCode", this.form, response => {
        console.log(response);
        window.sessionStorage.setItem("code", response.data.code);
      });
    },
    /** 验证码倒计时 */
    validateBtn() {
      //倒计时
      let time = 60;
      let timer = setInterval(() => {
        if (time == 0) {
          clearInterval(timer);
          this.disabled = false;
          this.btnTitle = "重新获取";
        } else {
          this.btnTitle = time + "秒后重试";
          this.disabled = true;
          time--;
        }
      }, 1000);
    },
    /** 注册 */
    register(form) {
      this.$refs[form].validate(valid => {
        if (valid) {
          this.$request.post("Login/Register", this.form, response => {
            if (response.data == true) {
              this.$message({
                message: "恭喜你，账号注册成功！",
                type: "success"
              });
              //自动跳转
              let time = 1;
              let timer = setInterval(() => {
                if (time == 0) {
                  this.$router.push({
                    path: "/Home"
                  });
                } else {
                  time--;
                }
              }, 1000);
            }
          });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    }
  }
};
</script>
<style scoped>
.login-wrap {
  position: relative;
  width: 100%;
  height: 100%;
  background-image: url(../../assets/img/pj1.jpg);
  background-size: 100%;
}
.ms-title {
  width: 100%;
  line-height: 50px;
  text-align: center;
  font-size: 20px;
  color: #fff;
  border-bottom: 1px solid #ddd;
}
.ms-login {
  position: absolute;
  left: 50%;
  top: 50%;
  width: 400px;
  margin: -190px 0 0 -175px;
  border-radius: 5px;
  background: rgba(255, 255, 255, 0.3);
  overflow: hidden;
}
.ms-content {
  padding: 30px 30px;
}
.login-btn {
  text-align: center;
}
.login-btn button {
  width: 100%;
  height: 36px;
  margin-bottom: 10px;
}
.register-tips {
  font-size: 12px;
  line-height: 30px;
  color: #fff;
  float: left;
}
.forget-tips {
  font-size: 12px;
  line-height: 30px;
  color: #fff;
  float: right;
}
</style>
