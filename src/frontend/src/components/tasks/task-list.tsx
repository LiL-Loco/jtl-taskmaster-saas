'use client'

import { DragDropContext, Droppable, Draggable } from '@hello-pangea/dnd'
import { GripVertical, X } from 'lucide-react'
import { Card, CardContent } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { TaskForm } from './task-form'

interface Task {
  id: string
  type: string
  parameters: string
  order: number
  isEnabled: boolean
}

interface TaskListProps {
  tasks: Task[]
  onChange: (tasks: Task[]) => void
}

export function TaskList({ tasks, onChange }: TaskListProps) {
  const onDragEnd = (result: any) => {
    if (!result.destination) return

    const items = Array.from(tasks)
    const [reorderedItem] = items.splice(result.source.index, 1)
    items.splice(result.destination.index, 0, reorderedItem)

    const reorderedTasks = items.map((task, index) => ({
      ...task,
      order: index
    }))

    onChange(reorderedTasks)
  }

  const removeTask = (taskId: string) => {
    const updatedTasks = tasks
      .filter(task => task.id !== taskId)
      .map((task, index) => ({
        ...task,
        order: index
      }))
    onChange(updatedTasks)
  }

  const addTask = (task: Omit<Task, 'id' | 'order'>) => {
    const newTask = {
      ...task,
      id: crypto.randomUUID(),
      order: tasks.length
    }
    onChange([...tasks, newTask])
  }

  return (
    <div className="space-y-4">
      <DragDropContext onDragEnd={onDragEnd}>
        <Droppable droppableId="tasks">
          {(provided) => (
            <div {...provided.droppableProps} ref={provided.innerRef}>
              {tasks.map((task, index) => (
                <Draggable
                  key={task.id}
                  draggableId={task.id}
                  index={index}
                >
                  {(provided) => (
                    <Card
                      ref={provided.innerRef}
                      {...provided.draggableProps}
                      className="mb-4"
                    >
                      <CardContent className="flex items-center p-4">
                        <div
                          {...provided.dragHandleProps}
                          className="mr-2 cursor-move"
                        >
                          <GripVertical className="h-5 w-5 text-gray-500" />
                        </div>
                        <div className="flex-1">
                          <h4 className="font-medium">{task.type}</h4>
                          <p className="text-sm text-gray-500">
                            {task.parameters}
                          </p>
                        </div>
                        <Button
                          variant="ghost"
                          size="sm"
                          onClick={() => removeTask(task.id)}
                        >
                          <X className="h-4 w-4" />
                        </Button>
                      </CardContent>
                    </Card>
                  )}
                </Draggable>
              ))}
              {provided.placeholder}
            </div>
          )}
        </Droppable>
      </DragDropContext>

      <TaskForm onSubmit={addTask} />
    </div>
  )
}