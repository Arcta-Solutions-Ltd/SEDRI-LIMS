import { useEffect } from 'react'

export default function GlobalErrorHandler() {
  useEffect(() => {
    const handler = (e) => {
      if (
        e.message &&
        e.message.includes('ResizeObserver loop completed with undelivered notifications')
      ) {
        e.stopImmediatePropagation()
      }
    }
    window.addEventListener('error', handler, true)
    return () => window.removeEventListener('error', handler, true)
  }, [])
}
