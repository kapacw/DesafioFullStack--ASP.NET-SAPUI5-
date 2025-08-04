// Task.controller.js
sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/m/MessageToast",
    "sap/ui/core/Fragment",
    "tarefas/front/service/Task.service",
    "tarefas/front/service/Sync.service",
    "tarefas/front/util/formatter",
    "tarefas/front/model/Task.model"
], function (Controller, MessageToast, Fragment, TaskService, SyncService, formatter, TaskModel) {
    "use strict";

    const DELAY_MS = 500;

    return Controller.extend("tarefas.front.controller.Task", {
        formatter: formatter,
        _timeout: null,
        _params: {
            page: 1,
            pageSize: 10,
            title: "",
            sort: "title",
            order: "asc"
        },

        onInit: function () {
            this.oModel = TaskModel.createTaskModel();
            this.getView().setModel(this.oModel, "TarefaModel");
            this._carregarTarefas();
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

        _carregarTarefas: async function () {
            await this._abrirBusy();
            try {
                await new Promise(resolve => setTimeout(resolve, DELAY_MS));
                const data = await TaskService.getAllTask(this._params);
                this.oModel.setProperty("/items", data.items);
                this.oModel.setProperty("/page", data.page);
                this.oModel.setProperty("/totalPages", Math.ceil(data.totalItems / this._params.pageSize));
                this.oModel.setProperty("/order", this._params.order);
            } catch (e) {
                MessageToast.show("Erro ao carregar tarefas");
            } finally {
                this._fecharBusy();
            }
        },

        onSearchLiveChange: function (oEvent) {
            const value = oEvent.getParameter("newValue");
            clearTimeout(this._timeout);
            this._timeout = setTimeout(() => {
                this._params.title = value;
                this._params.page = 1;
                this._carregarTarefas();
            }, 4000);
        },

        onPageSizeChange: async function (oEvent) {
            const selected = parseInt(oEvent.getSource().getSelectedKey());
            this._params.pageSize = selected === -1 ? 99999 : selected;
            this._params.page = 1;
            await new Promise(resolve => setTimeout(resolve, DELAY_MS));
            this._carregarTarefas();
        },

        onSortChange: async function (oEvent) {
            this._params.sort = oEvent.getSource().getSelectedKey();
            await new Promise(resolve => setTimeout(resolve, DELAY_MS));
            this._carregarTarefas();
        },

        onToggleOrder: async function () {
            this._params.order = this._params.order === "asc" ? "desc" : "asc";
            this.oModel.setProperty("/order", this._params.order);

            await new Promise(resolve => setTimeout(resolve, 500));
            this._carregarTarefas();
        },

        onPageAnterior: async function () {
            if (this._params.page > 1) {
                this._params.page--;
                await new Promise(resolve => setTimeout(resolve, DELAY_MS));
                this._carregarTarefas();
            }
        },

        onPageProxima: async function () {
            this._params.page++;
            await new Promise(resolve => setTimeout(resolve, DELAY_MS));
            this._carregarTarefas();
        },

        onToggleStatus: async function (oEvent) {
            const id = oEvent.getSource().getCustomData()[0].getValue();
            const newState = oEvent.getParameter("state");

            await this._abrirBusy();
            try {
                await new Promise(resolve => setTimeout(resolve, DELAY_MS));
                await TaskService.putTaskStatus(id, newState);
                MessageToast.show("Status atualizado com sucesso!");
                this._carregarTarefas();
            } catch (e) {
                // Reverte visualmente o switch em caso de erro
                const model = this.getView().getModel("TarefaModel");
                const items = model.getProperty("/items");

                const index = items.findIndex(item => item.id === id);
                if (index !== -1) {
                    items[index].completed = !newState; // reverte o estado visual
                    model.setProperty("/items", items); // atualiza o modelo
                }

                // Mostra mensagem do backend
                const msg = e?.message || "Erro ao atualizar status";
                MessageToast.show(msg);
            } finally {
                this._fecharBusy();
            }
        },

        onSyncBanco: async function () {
            await this._abrirBusy();
            try {
                await new Promise(resolve => setTimeout(resolve, DELAY_MS));
                await SyncService.sincronizarBanco();
                MessageToast.show("Banco sincronizado com sucesso!");
                this._carregarTarefas();
            } catch (e) {
                MessageToast.show("Erro ao sincronizar banco");
            } finally {
                this._fecharBusy();
            }
        },

        onDetalhes: async function (oEvent) {
            const oBindingContext = oEvent.getSource().getBindingContext("TarefaModel");
            if (oBindingContext) {
                const id = oBindingContext.getProperty("id");
                if (id !== undefined && id !== null) {
                    await new Promise(resolve => setTimeout(resolve, DELAY_MS));
                    const oRouter = this.getOwnerComponent().getRouter();
                    oRouter.navTo("taskDetails", { id: String(id) });
                } else {
                    MessageToast.show("ID da tarefa não encontrado no binding");
                }
            } else {
                MessageToast.show("Binding context não disponível");
            }
        }
    });
});
