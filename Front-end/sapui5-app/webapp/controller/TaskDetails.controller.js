sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/m/MessageToast",
    "sap/ui/core/Fragment",
    "tarefas/front/service/Task.service",
    "sap/ui/core/routing/History",
    "tarefas/front/util/formatter",
    "tarefas/front/model/TaskDetails.model"
], function (Controller, MessageToast, Fragment, TaskService, History, formatter, TaskDetailsModel) {
    "use strict";

    const DELAY_MS = 500;

    return Controller.extend("tarefas.front.controller.TaskDetails", {

        formatter: formatter,

        onInit: function () {
            const oModel = TaskDetailsModel.createTaskDetailsModel();
            this.getView().setModel(oModel, "TaskDetailsModel");

            this.getOwnerComponent()
                .getRouter()
                .getRoute("taskDetails")
                .attachPatternMatched(this.onRouteMatched, this);
        },

        _abrirBusy: async function () {
            if (!this._busyDialog) {
                this._busyDialog = await Fragment.load({
                    name: "tarefas.front.view.fragment.BusyDialog",
                    controller: this
                });
                this.getView().addDependent(this._busyDialog);
            }
            this._busyDialog.open();
        },

        _fecharBusy: function () {
            if (this._busyDialog) {
                this._busyDialog.close();
            }
        },

        onRouteMatched: async function (oEvent) {
            const params = oEvent.getParameter("arguments");

            if (!params || !params.id) {
                MessageToast.show("ID não informado na rota");
                return;
            }

            await this._abrirBusy();
            try {
                const data = await TaskService.getTaskById(params.id);
                this.getView().getModel("TaskDetailsModel").setData(data);
                this.getView().getModel("TaskDetailsModel").refresh(true);
                console.log("📊 Modelo atual:", this.getView().getModel("TaskDetailsModel").getData());
            } catch (e) {
                console.error("Erro na requisição:", e);
                MessageToast.show("Erro ao carregar detalhes");
            } finally {
                this._fecharBusy();
            }
        },

        onToggleStatus: async function () {
            const tarefa = this.getView().getModel("TaskDetailsModel").getData();
            await this._abrirBusy();
            try {
                await new Promise(resolve => setTimeout(resolve, DELAY_MS));
                await TaskService.putTaskStatus(tarefa.id);
                MessageToast.show("Status atualizado!");
                const data = await TaskService.getTaskById(tarefa.id);
                this.getView().getModel("TaskDetailsModel").setProperty("/id", data.id);
                this.getView().getModel("TaskDetailsModel").setProperty("/title", data.title);
                this.getView().getModel("TaskDetailsModel").setProperty("/completed", data.completed);
                this.getView().getModel("TaskDetailsModel").setProperty("/userId", data.userId);
            } catch (e) {
                MessageToast.show("Erro ao atualizar status");
            } finally {
                this._fecharBusy();
            }
        },

        onVoltar: function () {
            const oHistory = History.getInstance();
            const sPreviousHash = oHistory.getPreviousHash();
            const oRouter = this.getOwnerComponent().getRouter();

            if (sPreviousHash) {
                window.history.go(-1);
            } else {
                oRouter.navTo("taskList", {}, true);
            }
        }
    });
});