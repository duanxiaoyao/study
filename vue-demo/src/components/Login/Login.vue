<template>
    <div class="login-wrap">
        <div class="ms-login">
            <div class="ms-title">我好了，你呢</div>
            <el-form :model="form" ref="form" :rules="rules" class="ms-content">
                <el-form-item prop="loginCode">
                    <el-input v-model="form.loginCode" placeholder="账号">
                        <svg-icon slot="prepend" icon-class="user" />
                        <el-button slot="prepend" icon="el-icon-lx-people"></el-button>
                    </el-input>
                </el-form-item>
                <el-form-item v-if="visible" prop="passWord">
                    <el-input type="password" v-model="form.passWord" placeholder="密码">
                        <svg-icon slot="prepend" icon-class="pwd" />
                        <el-button slot="append" class="show-pwd" @click="showPwd('show')">
                            <svg-icon icon-class="eye" />
                        </el-button>
                    </el-input>
                </el-form-item>
                <el-form-item v-else prop="passWord">
                    <el-input type="text" v-model="form.passWord" placeholder="密码">
                        <svg-icon slot="prepend" icon-class="pwd" />
                        <el-button slot="append" class="show-pwd" @click="showPwd('hide')">
                            <svg-icon icon-class="eye-open" />
                        </el-button>
                    </el-input>
                </el-form-item>
                <div class="login-btn">
                    <el-button type="primary" @click="login('form')">登陆</el-button>
                    <!-- <el-button @click="register()">注册</el-button> -->
                </div>
                <p class="register-tips" @click="register()">注册账号</p>
                <p class="forget-tips" @click="forget()">忘记密码</p>
            </el-form>
        </div>
    </div>
</template>

<script>
export default {
    /** 组件名称 */
    name: "Login",
    /** 绑定数据 */
    data() {
        var validateIdName = (rule, value, callback) => {
            if (value === "") {
                callback(new Error("请输入账号"));
            } else {
                callback();
            }
        };
        var validateIdPassWord = (rule, value, callback) => {
            if (value === "") {
                callback(new Error("请输入密码"));
            } else {
                callback();
            }
        };
        return {
            /** 表单数据 */
            form: {
                loginCode: "",
                passWord: ""
            },
            rules: {
                loginCode: [{ validator: validateIdName, trigger: "blur" }],
                passWord: [{ validator: validateIdPassWord, trigger: "blur" }]
            },
            visible: true
        };
    },
    /** 回车事件 */
    created() {
        var _this = this;
        document.onkeydown = function(e) {
            var key = window.event.keyCode;
            if (key == 13) {
                _this.login("form");
            }
        };
    },
    methods: {
        /** 显示密码 */
        showPwd(value) {
            this.visible = !(value === "show");
        },
        /** 登陆 */
        login(form) {
            this.$refs[form].validate(valid => {
                if (valid) {
                    this.$request.post(
                        "Login/CheckUser",
                        this.form,
                        response => {
                            if (response.data == true) {
                                this.$message({
                                    message: "欢迎来到德莱联盟！",
                                    type: "success"
                                });
                                this.$router.push({
                                    path: "/Home"
                                });
                            } else {
                                this.$message({
                                    message: "账号或密码错误！",
                                    type: "info"
                                });
                            }
                        }
                    );
                } else {
                    console.log("error submit!!");
                    return false;
                }
            });
        },
        /** 注册 */
        register() {
            let url = this.$router.resolve({ path: "/Register" });
            window.open(url.href, "_blank");
        }
    }
};
</script>
<style scoped>
.login-wrap {
    position: relative;
    width: 100%;
    height: 100%;
    background-image: url(../../assets/img/pj.jpg);
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
