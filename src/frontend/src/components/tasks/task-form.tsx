'use client'

import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import * as z from 'zod'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Textarea } from '@/components/ui/textarea'
import { Button } from '@/components/ui/button'
import { Switch } from '@/components/ui/switch'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'

const taskTypes = [
  { id: 'jtl-ameise-import', name: 'JTL-Ameise Import' },
  { id: 'jtl-ameise-export', name: 'JTL-Ameise Export' },
  { id: 'versanddaten-import', name: 'Versanddaten Import' },
  { id: 'ftp-download', name: 'FTP Download' },
  { id: 'ftp-upload', name: 'FTP Upload' },
  { id: 'email-send', name: 'Send Email' },
  { id: 'process-start', name: 'Start Process' },
  { id: 'sql-query', name: 'SQL Query' }
]

const taskFormSchema = z.object({
  type: z.string(),
  parameters: z.string(),
  isEnabled: z.boolean().default(true)
})

type TaskFormValues = z.infer<typeof taskFormSchema>

interface TaskFormProps {
  onSubmit: (data: TaskFormValues) => void
}

export function TaskForm({ onSubmit }: TaskFormProps) {
  const form = useForm<TaskFormValues>({
    resolver: zodResolver(taskFormSchema),
    defaultValues: {
      type: '',
      parameters: '',
      isEnabled: true
    }
  })

  const handleSubmit = (data: TaskFormValues) => {
    onSubmit(data)
    form.reset()
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Add Task</CardTitle>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-4">
            <FormField
              control={form.control}
              name="type"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Task Type</FormLabel>
                  <Select
                    onValueChange={field.onChange}
                    defaultValue={field.value}
                  >
                    <FormControl>
                      <SelectTrigger>
                        <SelectValue placeholder="Select a task type" />
                      </SelectTrigger>
                    </FormControl>
                    <SelectContent>
                      {taskTypes.map((type) => (
                        <SelectItem key={type.id} value={type.id}>
                          {type.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="parameters"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Parameters (JSON)</FormLabel>
                  <FormControl>
                    <Textarea {...field} />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="isEnabled"
              render={({ field }) => (
                <FormItem className="flex items-center justify-between rounded-lg border p-4">
                  <FormLabel className="text-base">Enabled</FormLabel>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                </FormItem>
              )}
            />

            <Button type="submit">Add Task</Button>
          </form>
        </Form>
      </CardContent>
    </Card>
  )
}