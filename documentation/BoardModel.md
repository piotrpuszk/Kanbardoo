BoardModel:
- id: int32,
- name: string,
- owner: UserModel,
- creation date: DateTime,
- start date: DateTime,
- finish date: DateTime,
- status: StatusModel,
- background image url: string,
- tables: List\<TableModel>
- IValidatable

UserModel:
- id: int32,
- name: string,
- creation date: DateTime,
- IValidatable,

StatusModel:
- id: int32,
- name: string,
- IValidatable,

TableModel:
- id: int32,
- board id: int32,
- name: string,
- creation Date: DateTime,
- tasks: List\<TaskModel>
- IValidatable,

TaskModel:
- id: int32,
- table id: int32,
- name: string,
- description: string,
- due date: DateTime,
- status: StatusModel,
- assignee: UserModel,
- IValidatable,